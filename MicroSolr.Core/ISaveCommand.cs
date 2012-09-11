// -----------------------------------------------------------------------
// <copyright file="ISaveCommand.cs" company="Imran Saeed">
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
    public interface ISaveCommand<TData> : ICommand
    {
        IEnumerable<TData> Data { get; set; }
    }
}
