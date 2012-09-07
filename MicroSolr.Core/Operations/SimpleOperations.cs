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

        public override IEnumerable<TOutput> Load<TOutput>(ILoadCommand command, IDataSerializer<TOutput> serializer, IResponseFormatter<TOutput> formatter)
        {
            string loadQS = MakeLoadQueryString(command);
            Uri loadUri = MakeUri(SelectUri, loadQS);
            string response = _httpHelper.Get(loadUri);
            return serializer.DeSerialize(response, command.ResponseFormat);
        }

        public override IOperations Save<TData>(ISaveCommand<TData> command, IDataSerializer<TData> serializer, bool commit = true, bool optimize = false)
        {
            _httpHelper.Post(UpdateUri, serializer.Serialize(command.Data, FormatType.JSON), "application/json", Encoding.UTF8);
            if (commit) Commit();
            if (optimize) Optimize();
            return this;
        }
    }
}
