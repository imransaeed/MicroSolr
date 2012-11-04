// -----------------------------------------------------------------------
// <copyright file="IResponseFormatter.cs" company="Imran Saeed">
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
    /// <summary>
    /// A response formatter that is applied on Solr response
    /// </summary>
    /// <typeparam name="TFormattedResponse">The type of the formatted response.</typeparam>
    public interface IResponseFormatter<out TFormattedResponse>
    {
        /// <summary>
        /// Formats the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        TFormattedResponse Format(string data);
    }
}