// -----------------------------------------------------------------------
// <copyright file="HttpClient.cs" company="EF">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Core.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HttpClient : IClient
    {
        public HttpClient(Uri baseUri, params ICore[] cores)
        {
            BaseUri = baseUri;
            Cores = new List<ICore>(cores);
            DefaultCore = Cores.First();
        }

        public Uri BaseUri
        {
            get;
            private set;
        }

        public IList<ICore> Cores
        {
            get;
            private set;
        }

        public ICore DefaultCore
        {
            get;
            private set;
        }
    }
}
