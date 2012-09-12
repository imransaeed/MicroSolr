// -----------------------------------------------------------------------
// <copyright file="SingleCore.cs" company="Imran Saeed">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Core.Cores
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MicroSolr.Core.Commands;
    using MicroSolr.Core.Operations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SingleCore : ICore
    {
        public SingleCore(string name, IClient client, IOperations operations = null)
        {
            Name = name;
            Client = client;
            Operations = operations ?? new SimpleOperations(this);
        }

        public string Name
        {
            get;
            private set;
        }

        public IOperations Operations
        {
            get;
            private set;
        }

        public IClient Client
        {
            get;
            private set;
        }


        public ILoadCommand CreateLoadCommand()
        {
            return new LoadCommand();
        }

        public ISaveCommand<TData> CreateSaveCommand<TData>()
        {
            return new SaveCommand<TData>() ;
        }
    }
}
