// -----------------------------------------------------------------------
// <copyright file="SimpleOperations.cs" company="EF">
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
    public class SimpleOperations : BaseOperations
    {
        public SimpleOperations(IHttpHelper httpHelper)
            : base(httpHelper)
        {

        }

        public override TOutput Load<TOutput>(ILoadCommand command, IResponseFormatter<TOutput> formatter)
        {
            string lsq = MakeLoadQueryString(command);
            Uri loadUri = MakeUri(SelectUri, lsq);
            string response = _httpHelper.HttpCommunicate(loadUri, null, null, null, false);
            return formatter.Format(response);
        }

        public override IOperations Save<TData>(ISaveCommand<TData> command, bool commit = true, bool optimize = false)
        {
            TData data = command.Data;
            if (commit) Commit();
            if (optimize) Optimize();
            return this;
        }
    }
}
