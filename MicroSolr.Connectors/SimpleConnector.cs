// -----------------------------------------------------------------------
// <copyright file="SimpleConnector.cs" company="Imran Saeed">
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
    using Core;
    using Core.Clients;
    using Core.Cores;
    using Core.Operations;
    using System;

    /// <summary>
    /// Simple Solr connection class that uses Json Serialization to load and save data
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    public class SimpleConnector<TData> : BaseConnector<TData>, ISimpleConnector<TData>
    {
        /// <summary>
        /// Creates a simple connector instance from the url and core name
        /// </summary>
        /// <param name="serverUrl">Base url of the server without core name</param>
        /// <param name="coreName">Core name</param>
        /// <returns></returns>
        public static ISimpleConnector<TData> Create(string serverUrl, string coreName)
        {
            var client = new HttpClient(new Uri(serverUrl));
            return new SimpleConnector<TData>(client, coreName);
        }

        public SimpleConnector(IClient client, string coreName, IDataSerializer<TData> serializer = null)
            :base(client, serializer)
        {
            AssembleConnector(coreName);
        }

        protected override ICore CreateCore(string coreName, IClient client)
        {
            return new SingleCore(coreName, client, new SimpleOperations(client.BaseUri, coreName, HttpHelper));
        }
    }
}
