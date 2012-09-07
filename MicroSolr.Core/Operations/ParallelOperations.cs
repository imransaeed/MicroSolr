// -----------------------------------------------------------------------
// <copyright file="ParallelOperations.cs" company="EF">
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
    public class ParallelOperations : BaseOperations
    {
        public ParallelOperations(IHttpHelper httpHelper)
            : base(httpHelper)
        {

        }
        public override TOutput Load<TOutput>(ILoadCommand command, IResponseFormatter<TOutput> formatter)
        {
            throw new NotImplementedException();
        }

        public override IOperations Save<TData>(ISaveCommand<TData> command, bool commit = true, bool optimize = false)
        {
            throw new NotImplementedException();
        }

    }
}
