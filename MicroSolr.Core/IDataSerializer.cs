// -----------------------------------------------------------------------
// <copyright file="IDataSerializer.cs" company="Microsoft">
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
    using System.Collections.Generic;

    /// <summary>
    /// Serializer for generating Solr compatible streams.
    /// </summary>
    public interface IDataSerializer<TData>
    {
        /// <summary>
        /// Serializes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        string Serialize(TData data, FormatType format);

        /// <summary>
        /// Serializes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        string Serialize(IEnumerable<TData> data, FormatType format);

        /// <summary>
        /// Deserializes the data from the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        IEnumerable<TData> DeSerialize(string stream, FormatType format);
    }
}
