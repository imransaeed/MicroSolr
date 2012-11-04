// -----------------------------------------------------------------------
// <copyright file="HttpHelpers.cs" company="Imran Saeed">
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
    using System.Text;

    /// <summary>
    /// Defines a HTTP Helper
    /// </summary>
    public interface IHttpHelper
    {
        /// <summary>
        /// Gets the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        string Get(Uri uri);

        /// <summary>
        /// Posts text content to specified URI.
        /// </summary>
        /// <param name="uri">The HTTP based URI.</param>
        /// <param name="content">Text content.</param>
        /// <param name="contentType">Type of the content in server compatible format.</param>
        /// <param name="bytesConverter">The bytes converter for content.</param>
        /// <returns></returns>
        string Post(Uri uri, string content, string contentType, Encoding bytesConverter);
    }
}
