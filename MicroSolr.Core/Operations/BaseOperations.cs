// -----------------------------------------------------------------------
// <copyright file="BaseOperations.cs" company="EF">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Core.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class BaseOperations : IOperations
    {
        protected IHttpHelper _httpHelper;

        public abstract TOutput Load<TOutput>(ILoadCommand command, IResponseFormatter<TOutput> formatter);

        public abstract IOperations Save<TData>(ISaveCommand<TData> command, bool commit = true, bool optimize = false);

        public BaseOperations(IHttpHelper httpHelper)
        {
            _httpHelper = httpHelper;
            CoreUri = new Uri(string.Format("{0}/{1}/", Core.Server.BaseUri.ToString(), Core.Name));
            SelectUri = new Uri(CoreUri.ToString() + "select/");
            UpdateUri = new Uri(CoreUri.ToString() + "update/");
        }

        public virtual IOperations Commit()
        {
            Uri u = MakeUri(UpdateUri, "commit=true");
            _httpHelper.HttpCommunicate(u, null, null, null, false);
            return this;
        }

        public virtual IOperations Optimize()
        {
            Uri u = MakeUri(UpdateUri, "optimize=true");
            _httpHelper.HttpCommunicate(u, null, null, null, false);
            return this;
        }

        public virtual ICore Core
        {
            get;
            private set;
        }

        protected Uri CoreUri
        {
            get;
            private set;
        }

        protected Uri SelectUri
        {
            get;
            private set;
        }

        protected Uri UpdateUri
        {
            get;
            private set;
        }

        protected static Uri MakeUri(Uri baseUri, string queryString)
        {
            UriBuilder builder = new UriBuilder(baseUri);
            builder.Query = queryString;
            return builder.Uri;
        }

        protected static string MakeLoadQueryString(ILoadCommand command)
        {
            IDictionary<string, string> qsParts = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(command.Query))
            {
                qsParts.Add("q", command.Query);
            }

            qsParts.Add("wt", Enum.GetName(typeof(FormatType), command.ResponseFormat).ToLowerInvariant());

            var qList = from k in qsParts.Keys select string.Format("{0}={1}", k, qsParts[k]);

            return string.Join("&", qList);
        }
    }
}
