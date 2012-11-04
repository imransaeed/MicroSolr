// -----------------------------------------------------------------------
// <copyright file="IOperations.cs" company="Imran Saeed">
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
    /// Defines the operations supported by a core
    /// </summary>
    public interface IOperations
    {
        /// <summary>
        /// Loads the specified command.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="serializer">The serializer.</param>
        /// <param name="formatter">The formatter.</param>
        /// <returns></returns>
        IEnumerable<TOutput> Load<TOutput>(ILoadCommand command, IDataSerializer<TOutput> serializer, IResponseFormatter<string> formatter);

        /// <summary>
        /// Saves the specified command.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="serializer">The serializer.</param>
        /// <param name="commit">if set to <c>true</c> [commit].</param>
        /// <param name="optimize">if set to <c>true</c> [optimize].</param>
        /// <returns></returns>
        IOperations Save<TData>(ISaveCommand<TData> command, IDataSerializer<TData> serializer, bool commit = true, bool optimize = false);

        /// <summary>
        /// Sends a commit command to the core.
        /// </summary>
        /// <returns></returns>
        IOperations Commit();

        /// <summary>
        /// Starts an optimization instance on the core.
        /// </summary>
        /// <returns></returns>
        IOperations Optimize();
    }
}
