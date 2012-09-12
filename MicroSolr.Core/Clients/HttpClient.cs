﻿// -----------------------------------------------------------------------
// <copyright file="HttpClient.cs" company="Imran Saeed">
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

namespace MicroSolr.Core.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HttpClient : IClient
    {
        public HttpClient(Uri baseUri)
        {
            BaseUri = baseUri;
            Cores = new List<ICore>();

        }

        public void AddCores(params ICore[] cores)
        {
            foreach (var core in cores)
                Cores.Add(core);

            DefaultCore = Cores.First();
        }

        public Uri BaseUri
        {
            get;
            private set;
        }

        public IList<ICore> Cores
        {
            get;
            private set;
        }

        public ICore DefaultCore
        {
            get;
            private set;
        }
    }
}