// -----------------------------------------------------------------------
// <copyright file="DynamicConnector.cs" company="Imran Saeed">
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
    using MicroSolr.Core.Clients;
    using MicroSolr.Core;
    using MicroSolr.Core.Cores;
    using MicroSolr.Core.Operations;

    /// <summary>
    /// A dynamic connector class that will use parallel operations for queries
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    public class DynamicConnector<TData> : BaseConnector<TData>, IDynamicConnector<TData>
    {
        public static IDynamicConnector<TData> Create(string serverUrl, params string[] coreNames)
        {
            return new DynamicConnector<TData>(new HttpClient(new Uri(serverUrl)), null, coreNames);
        }

        public DynamicConnector(IClient client, IDataSerializer<TData> serializer, params string[] coreNames)
            : base(client, serializer)
        {
            base.AssembleConnector(coreNames);
        }

        protected override ICore CreateCore(string coreName, IClient client)
        {
            return new SingleCore(coreName, client, new ParallelOperations(client.BaseUri, coreName, HttpHelper));
        }
    }
}
