// -----------------------------------------------------------------------
// <copyright file="IClient.cs" company="Imran Saeed">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Core
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IClient
    {
        Uri BaseUri { get; }

        IList<ICore> Cores { get; }

        ICore DefaultCore { get; }
    }
}
