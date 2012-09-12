// -----------------------------------------------------------------------
// <copyright file="ParallelOperations.cs" company="Imran Saeed">
// Copyright (c) 2012 Imran Saeed
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Core.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ParallelOperations : SimpleOperations
    {
        private readonly long _readSplitSize;
        private readonly long _writeSplitSize;

        public ParallelOperations(ICore core, IHttpHelper httpHelper = null, long readSplitSize = 1000, long writeSplitSize = 1000)
            : base(core, httpHelper)
        {
            _readSplitSize = readSplitSize;
            _writeSplitSize = writeSplitSize;
        }

        public override IEnumerable<TOutput> Load<TOutput>(ILoadCommand command, IDataSerializer<TOutput> serializer, IResponseFormatter<string> formatter)
        {
            long maxRows = GetRowCountForResults(command);

            if (maxRows > _readSplitSize)
            {
                List<TOutput> results = new List<TOutput>();
                List<string> commands = new List<string>();

                for (long startIndex = command.StartIndex, batchNum = 0; startIndex < maxRows; startIndex += (batchNum * _readSplitSize), batchNum++)
                {
                    ILoadCommand copyCommand = command.Clone() as ILoadCommand;
                    copyCommand.GetAll = false;
                    copyCommand.StartIndex = startIndex;
                    copyCommand.MaxRows = _readSplitSize;

                    string batchCommand = MakeLoadQueryString(copyCommand);

                    commands.Add(batchCommand);
                }
                commands.AsParallel().ForAll(s =>
                {
                    var batchResults = ExecuteLoad(s, command.ResponseFormat, serializer, formatter);
                    lock (results)
                    {
                        results.AddRange(batchResults);
                    }
                });

                return results;
            }
            else
            {
                //Downgrade to simple operations
                return base.Load(command, serializer, formatter);
            }
        }

        public override IOperations Save<TData>(ISaveCommand<TData> command, IDataSerializer<TData> serializer, bool commit = true, bool optimize = false)
        {
            long dataLength = command.Data.LongCount();
            if (dataLength > _writeSplitSize)
            {
                IList<long> batchNumbers = new List<long>();
                for (long startRow = 0, batchNumber = 0; startRow < dataLength; startRow += _writeSplitSize, batchNumber++)
                {
                    batchNumbers.Add(batchNumber);
                }

                batchNumbers.AsParallel().ForAll(b =>
                {
                    var data = from c in command.Data.Skip((int)(b * _writeSplitSize)).Take((int)_writeSplitSize) select c;
                    base.ExecuteSave(data, serializer, commit, optimize);
                });
                return this;
            }
            else
            {
                //Downgrade to simple operations
                return base.Save(command, serializer, commit, optimize);
            }
        }
    }
}
