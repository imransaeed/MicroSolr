// -----------------------------------------------------------------------
// <copyright file="ILoadCommand.cs" company="Imran Saeed">
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
    /// Defines a load command
    /// </summary>
    public interface ILoadCommand : ICommand, ICloneable
    {
        /// <summary>
        /// Gets or sets the response format.
        /// </summary>
        /// <value>
        /// The response format.
        /// </value>
        FormatType ResponseFormat { get; set; }

        /// <summary>
        /// Gets or sets the start index.
        /// </summary>
        /// <value>
        /// The start index.
        /// </value>
        long StartIndex { get; set; }

        /// <summary>
        /// Gets or sets the max rows.
        /// </summary>
        /// <value>
        /// The max rows.
        /// </value>
        long MaxRows { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [get all].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [get all]; otherwise, <c>false</c>.
        /// </value>
        bool GetAll { get; set; }

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>
        /// The query.
        /// </value>
        string Query { get; set; }

        /// <summary>
        /// Gets or sets the filter query.
        /// </summary>
        /// <value>
        /// The filter query.
        /// </value>
        string FilterQuery { get; set; }

        /// <summary>
        /// Gets or sets the field filter.
        /// </summary>
        /// <value>
        /// The field filter.
        /// </value>
        string FieldFilter { get; set; }
    }
}
