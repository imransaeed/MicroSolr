// -----------------------------------------------------------------------
// <copyright file="SimpleOperations.cs" company="Imran Saeed">
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

        public override IEnumerable<TOutput> Load<TOutput>(ILoadCommand command, IDataSerializer<TOutput> serializer, IResponseFormatter<string> formatter)
        {
            if (command.GetAll)
            {
                command.MaxRows = GetRowCountForResults(command);
            }

            if (command.MaxRows > 100000)
            {
                System.Diagnostics.Debug.WriteLine("Too many rows. Try using concurrent library.");
            }

            string loadQS = MakeLoadQueryString(command);

            return ExecuteLoad(loadQS, command.ResponseFormat, serializer, formatter);
        }

        public override IOperations Save<TData>(ISaveCommand<TData> command, IDataSerializer<TData> serializer, bool commit = true, bool optimize = false)
        {
            return ExecuteSave(command.Data, serializer, commit, optimize);
        }
    }
}
