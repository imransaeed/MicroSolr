// -----------------------------------------------------------------------
// <copyright file="LoadCommand.cs" company="Imran Saeed">
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

namespace MicroSolr.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LoadCommand : ILoadCommand
    {
        public FormatType ResponseFormat 
        {
            get;
            set;
        }

        public long StartIndex
        {
            get;
            set;
        }

        public long MaxRows
        {
            get;
            set;
        }

        public bool GetAll
        {
            get;
            set;
        }

        public string Query
        {
            get;
            set;
        }

        public string FilterQuery
        {
            get;
            set;
        }

        public string FieldFilter
        {
            get;
            set;
        }

        public object Clone()
        {
            return new LoadCommand { FieldFilter = this.FieldFilter, Query = this.Query, FilterQuery = this.FilterQuery, GetAll = this.GetAll, MaxRows = this.MaxRows, ResponseFormat = this.ResponseFormat, StartIndex = this.StartIndex };
        }
    }
}
