﻿// -----------------------------------------------------------------------
// <copyright file="BaseConnector.cs"  company="Imran Saeed">
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

namespace MicroSolr.Connectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MicroSolr.Core;
    using MicroSolr.Core.Cores;
    using MicroSolr.Core.Serializers;
    using MicroSolr.Core.Web;

    /// <summary>
    /// Base connector implementation
    /// </summary>
    public abstract class BaseConnector<TData> : IConnector<TData>
    {
        private IClient _client;
        private IDataSerializer<TData> _serializer;

        protected BaseConnector(IClient client, IDataSerializer<TData> serializer)
        {
            _client = client;
            _serializer = serializer ?? new MultiFormatSerializer<TData>();
            HttpHelper = new StatelessHttpHelper();
        }

        protected void AssembleConnector(params string[] coreNames)
        {
            foreach (string coreName in coreNames)
            {
                _client.Cores.Add(CreateCore(coreName, _client));
            }
        }

        /// <summary>
        /// Saves all the objects in the solr core. Commit will be  called automatically after all the objects are saved.
        /// </summary>
        /// <param name="items">List of items</param>
        public virtual void Save(params TData[] items)
        {
            ISaveCommand<TData> cmd = _client.DefaultCore.CreateSaveCommand<TData>();
            cmd.Data = items;
            _client.DefaultCore.Operations.Save<TData>(cmd, _serializer);
        }

        /// <summary>
        /// Queries the core and returns a list of matching objects
        /// </summary>
        /// <param name="query">Solr query (q=)</param>
        /// <param name="startIndex">Result start index</param>
        /// <param name="maxRows">Maximum rows to be returned</param>
        /// <param name="getAll">If <c>true</c> returns all the rows from the results. maxRows will be ignored when this is set to true.</param>
        /// <returns>List of matching objects.</returns>
        public virtual IEnumerable<TData> Query(string query, long startIndex = 0, long maxRows = 1000, bool getAll = false)
        {
            ILoadCommand cmd = _client.DefaultCore.CreateLoadCommand();
            cmd.Query = query;
            cmd.ResponseFormat = FormatType.JSON;
            cmd.StartIndex = startIndex;
            cmd.MaxRows = maxRows;
            cmd.GetAll = getAll;

            return _client.DefaultCore.Operations.Load<TData>(cmd, _serializer, null);
        }

        protected virtual IHttpHelper HttpHelper { get; private set; }

        protected abstract ICore CreateCore(string coreName, IClient client);
    }
}
