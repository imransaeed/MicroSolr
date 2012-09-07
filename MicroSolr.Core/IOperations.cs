// -----------------------------------------------------------------------
// <copyright file="IOperations.cs" company="EF">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IOperations
    {
        TOutput Load<TOutput>(ILoadCommand command, IResponseFormatter<TOutput> formatter);

        IOperations Save<TData>(ISaveCommand<TData> command, bool commit = true, bool optimize = false);

        IOperations Commit();

        IOperations Optimize();

        ICore Core { get; }
    }
}
