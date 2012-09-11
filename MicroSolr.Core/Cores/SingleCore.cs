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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SingleCore : ICore
    {
        public SingleCore(string name, IServer server, IOperations operations)
        {
            Name = name;
            Server = server;
            Operations = operations;
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

        public IServer Server
        {
            get;
            private set;
        }
    }
}
