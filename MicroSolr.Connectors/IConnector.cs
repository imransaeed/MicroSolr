// -----------------------------------------------------------------------
// <copyright file="IConnector.cs" company="Imran Saeed">
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IConnector<TData>
    {
        /// <summary>
        /// Queries the core and returns a list of matching objects
        /// </summary>
        /// <param name="query">Solr query (q=)</param>
        /// <param name="startIndex">Result start index</param>
        /// <param name="maxRows">Maximum rows to be returned</param>
        /// <param name="getAll">If <c>true</c> returns all the rows from the results. maxRows will be ignored when this is set to true.</param>
        /// <returns>List of matching objects.</returns>
        IEnumerable<TData> Query(string query, long startIndex = 0, long maxRows = 1000, bool getAll = false);

        /// <summary>
        /// Saves all the objects in the solr core. Commit will be  called automatically after all the objects are saved.
        /// </summary>
        /// <param name="items">List of items</param>
        void Save(params TData[] items);
    }
}
