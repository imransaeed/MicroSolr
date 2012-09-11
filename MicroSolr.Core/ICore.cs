// -----------------------------------------------------------------------
// <copyright file="ICore.cs" company="Imran Saeed">
// TODO: Update copyright text.
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

        IClient Server { get; }
    }
}
