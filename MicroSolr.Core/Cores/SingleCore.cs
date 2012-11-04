// -----------------------------------------------------------------------
// <copyright file="SingleCore.cs" company="Imran Saeed">
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

namespace MicroSolr.Core.Cores
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MicroSolr.Core.Commands;
    using MicroSolr.Core.Operations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SingleCore : ICore
    {
        public SingleCore(string name, IClient client, IOperations operations = null)
        {
            Name = name;
            Client = client;
            Operations = operations ?? new SimpleOperations(this.Client.BaseUri, name);
        }

        public string Name
        {
            get;
            private set;
        }

        public IOperations Operations
        {
            get;
            private set;
        }

        public IClient Client
        {
            get;
            private set;
        }


        public ILoadCommand CreateLoadCommand()
        {
            return new LoadCommand();
        }

        public ISaveCommand<TData> CreateSaveCommand<TData>()
        {
            return new SaveCommand<TData>() ;
        }
    }
}
