// -----------------------------------------------------------------------
// <copyright file="SingleCoreShardingStrategy.cs" company="Imran Saeed">
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
        private readonly IClient _server;
        public SingleCoreShardingStrategy(IClient server)
        {
            _server = server;
        }

        public ICore GetTargetCore(TData data)
        {
            return _server.DefaultCore;
        }
    }
}
