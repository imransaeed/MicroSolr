﻿// -----------------------------------------------------------------------
// <copyright file="ICore.cs" company="Imran Saeed">
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

namespace MicroSolr.Core
{
    using System;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICore
    {
        string Name { get; }

        IOperations Operations { get; }

        ILoadCommand CreateLoadCommand();

        ISaveCommand<TData> CreateSaveCommand<TData>();

        IClient Client { get; }
    }
}
