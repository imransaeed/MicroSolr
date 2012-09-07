// -----------------------------------------------------------------------
// <copyright file="IServer.cs" company="EF">
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
    public interface IServer
    {
        Uri BaseUri { get; }

        IList<ICore> Cores { get; }

        ICore DefaultCore { get; }
    }
}
