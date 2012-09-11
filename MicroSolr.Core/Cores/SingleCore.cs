﻿// -----------------------------------------------------------------------
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SingleCore : ICore
    {
        public SingleCore(string name, IClient server, IOperations operations)
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

        public IClient Server
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
