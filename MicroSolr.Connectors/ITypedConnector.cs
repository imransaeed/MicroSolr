namespace MicroSolr.Connectors
{
    using System;
    using System.Collections.Generic;

    public interface ITypedConnector<TCoreDocumentType>
    {
        void BulkAdd(string primaryKey, string[] bulkIds, string bulkIdFieldName, string[] additionalFieldNames, Func<string[]> getAdditionalFieldValues);
        void BulkDelete(string primaryKey, string[] bulkIds, string bulkIdFieldName);
        void BulkUpdate(string primaryKey, string[] bulkIds, string bulkIdFieldName, string[] additionalFieldNames = null, Func<string[]> getAdditionalFieldValues = null, bool commit = true, bool optimize = false);
        void Commit();
        void Optimize();
        void Save(System.Collections.Generic.IList<TCoreDocumentType> documents, bool commit = true, bool optimize = false);
        void Save(TCoreDocumentType document, bool commit = true, bool optimize = false);
        IList<TCoreDocumentType> Search(string query, bool getAll = true, long startIndex = 0, int maxRows = 100);
        string[] SearchIds(string query, long startIndex = 0, long maxRows = 100);
        IEnumerable<System.Xml.XmlNode> SearchXML(string query, bool getAll = true, long startIndex = 0, int maxRows = 100);
    }
}
