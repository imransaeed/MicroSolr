// -----------------------------------------------------------------------
// <copyright file="IDataSerializer.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Serializer for generating Solr compatible streams.
    /// </summary>
    public interface IDataSerializer<TData>
    {
        string Serialize(TData data, FormatType format);
        string Serialize(IEnumerable<TData> data, FormatType format);
        IEnumerable<TData> DeSerialize(string stream, FormatType format);
    }
}
