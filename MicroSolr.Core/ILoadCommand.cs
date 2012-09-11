// -----------------------------------------------------------------------
// <copyright file="ILoadCommand.cs" company="Imran Saeed">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Core
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ILoadCommand : ICommand, ICloneable
    {
        FormatType ResponseFormat { get; set; }

        long StartIndex { get; set; }

        long MaxRows { get; set; }

        bool GetAll { get; set; }

        string Query { get; set; }

        string FilterQuery { get; set; }

        string FieldFilter { get; set; }
    }
}
