// -----------------------------------------------------------------------
// <copyright file="SingleCoreShardingStrategy.cs" company="EF">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Core.ShardingStrategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SingleCoreShardingStrategy<TData> : IShardingStrategy<TData>
    {
        private readonly IServer _server;
        public SingleCoreShardingStrategy(IServer server)
        {
            _server = server;
        }

        public ICore GetTargetCore(TData data)
        {
            return _server.DefaultCore;
        }
    }
}
