// -----------------------------------------------------------------------
// <copyright file="SaveCommand.cs" company="EF">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SaveCommand<TData> : ISaveCommand<TData>
    {
        public IEnumerable<TData> Data
        {
            get;
            set;
        }

     


        public IShardingStrategy<TData> ShardingStrategy
        {
            get { throw new NotImplementedException(); }
        }
    }
}
