// -----------------------------------------------------------------------
// <copyright file="SimpleConnector.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Connectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using MicroSolr.Core;

    /// <summary>
    /// Simple Solr connection class that uses JSON Serialization to load and save data
    /// </summary>
    public class SimpleConnector<TData> : ISimpleConnector<TData>
    {
        private IClient _client;
        private IDataSerializer<TData> _serializer;

        public SimpleConnector(IClient client, IDataSerializer<TData> serializer)
        {
            _client = client;
            _serializer = serializer;
        }

        public void Save(params TData[] items)
        {
            ISaveCommand<TData> cmd = _client.DefaultCore.CreateSaveCommand<TData>();
            cmd.Data = items;
            _client.DefaultCore.Operations.Save<TData>(cmd, _serializer);
        }

        public IEnumerable<TData> Query(string query, long startIndex = 0, long maxRows = 1000, bool getAll = false)
        {
            ILoadCommand cmd = _client.DefaultCore.CreateLoadCommand();
            cmd.Query = query;
            cmd.ResponseFormat = FormatType.JSON;
            cmd.StartIndex = startIndex;
            cmd.MaxRows = maxRows;
            cmd.GetAll = getAll;

            return _client.DefaultCore.Operations.Load<TData>(cmd, _serializer, null);
        }
    }
}
