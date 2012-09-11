// -----------------------------------------------------------------------
// <copyright file="IConnector.cs" company="Imran Saeed">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Connectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Simple Solr connection class that uses JSON Serialization to load and save data
    /// </summary>
    public interface ISimpleConnector<TData>
    {
        /// <summary>
        /// Queries the core and returns a list of matching objects
        /// </summary>
        /// <param name="query">Solr query (q=)</param>
        /// <param name="startIndex">Result start index</param>
        /// <param name="maxRows">Maximum rows to be returned</param>
        /// <param name="getAll">If <c>true</c> returns all the rows from the results. maxRows will be ignored when this is set to true.</param>
        /// <returns>List of matching objects.</returns>
        IEnumerable<TData> Query(string query, long startIndex = 0, long maxRows = 1000, bool getAll = false);

        /// <summary>
        /// Saves all the objects in the solr core. Commit will be  called automatically after all the objects are saved.
        /// </summary>
        /// <param name="items"></param>
        void Save(params TData[] items);
    }
}
