﻿// -----------------------------------------------------------------------
// <copyright file="ParallelOperations.cs" company="EF">
// TODO: Update copyright text.
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

        public ParallelOperations(IHttpHelper httpHelper, long readSplitSize, long writeSplitSize)
            : base(httpHelper)
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
            if (command.Data.LongCount() > _writeSplitSize)
            {
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
