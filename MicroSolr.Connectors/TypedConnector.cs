using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroSolr.Connectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;

    using Newtonsoft.Json;
    using System.IO;
    using System.Xml.Linq;
    using System.Xml;
    
    /// <summary>
    /// A light weight class to query solr core
    /// </summary>
    public sealed class TypedConnector<TCoreDocumentType> : MicroSolr.Connectors.ITypedConnector<TCoreDocumentType>
    {
        #region JSonresponsecontainer
        private class Response
        {
            //[JsonProperty("docs")]
            public TCoreDocumentType[] docs { get; set; }
        }

        private class JsonResponse
        {
            [JsonProperty("numFound")]
            public long NumFound { get; set; }

            [JsonProperty("start")]
            public long Start { get; set; }

            [JsonProperty("response")]
            public Response Response { get; set; }
        }
        #endregion

        #region Private

        private readonly string _baseUrl;
        private readonly string _updateUrl;
        private readonly string _csvUpdateUrl;
        private readonly string _queryUrl;
        private readonly string _uniqueFieldName;


        private TResponse QueryCore<TResponse>(string query, Func<string, TResponse> responseFormatter)
        {
            UriBuilder requestUriBuilder = new UriBuilder(_queryUrl);
            requestUriBuilder.Query = query;
            string response = HttpHelper.HttpCommunicate(requestUriBuilder.Uri.ToString());
            return responseFormatter(response);
        }

        private static JsonResponse JsonResponseFormatter(string data)
        {
            return JsonConvert.DeserializeObject<JsonResponse>(data);
        }

        private static string[] CsvListResponseFormatter(string data)
        {
            return data.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
        }

        private void CommitAndOptimize(bool commit, bool optimize)
        {

            if (commit) Commit();
            if (optimize) Optimize();
        }


        private IEnumerable<TResult> ParallelResultsFetcher<TResult>(string query, long startIndex, long maxRows, bool getAll, string filterFieldName, Func<string, TResult[]> responseFormatter, HttpHelper.WriterType writerType)
        {
            string formattedQuery = !string.IsNullOrEmpty(filterFieldName) ? query + "&fl=" + filterFieldName : query;
            JsonResponse responseObject = QueryCore<JsonResponse>(HttpHelper.MakeSearchQuery(formattedQuery, HttpHelper.WriterType.JSON, startIndex, 0), JsonResponseFormatter);
            if (responseObject.NumFound > maxRows)
            {
                List<TResult> results = new List<TResult>();
                if (getAll)
                {
                    IList<string> queries = HttpHelper.MakeBatchSearchQueries(formattedQuery + "&omitHeader=true", startIndex, maxRows, responseObject.NumFound, writerType);
                    queries.AsParallel().ForAll(q =>
                    {
                        TResult[] batchResults = QueryCore<TResult[]>(q, responseFormatter);
                        lock (results)
                        {
                            results.AddRange(batchResults);
                        }
                    });
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Results are being ignored.");
                }

                return results;
            }
            else
            {
                return QueryCore<TResult[]>(HttpHelper.MakeSearchQuery(formattedQuery, writerType), responseFormatter);
            }
        }

        private void SaveObject(object document, bool commit, bool optimize)
        {
            HttpHelper.HttpCommunicate(_updateUrl, JsonConvert.SerializeObject(document), post: true);
            CommitAndOptimize(commit, optimize);
        }
        #endregion

        #region Public

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedConnector&lt;TCoreDocumentType&gt;"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL of your Solr core. For example http://localhost:8983/Solr/ </param>
        /// <param name="uniqueFieldName">Name of field that is the primary key.</param>
        public TypedConnector(string baseUrl, string uniqueFieldName = "")
        {
            _uniqueFieldName = uniqueFieldName;
            _baseUrl = new Uri(baseUrl.TrimEnd('/')).ToString(); // Normalizes the string
            _updateUrl = new Uri(_baseUrl + "/update/json/").ToString();
            _csvUpdateUrl = new Uri(_baseUrl + "/update/csv/").ToString();
            _queryUrl = new Uri(_baseUrl + "/select/").ToString();

        }

        /// <summary>
        /// Searches the core for specified query.
        /// </summary>
        /// <param name="query">The solr query without ?q= prefix. All other query related parameters like fq, wt are allowed.</param>
        /// <param name="getAll">Specifies whether to return all the rows or not.</param>
        /// <param name="startIndex">Start index of search results.</param>
        /// <param name="maxRows">The maximum number of rows to return in a call to solr.</param>
        /// <returns>A list of documents.</returns>
        public IList<TCoreDocumentType> Search(string query, bool getAll = true, long startIndex = 0, int maxRows = 100)
        {
            return ParallelResultsFetcher<TCoreDocumentType>(query, 0, maxRows, getAll, string.Empty,
                (data) => { return JsonResponseFormatter(data).Response.docs; }
                , HttpHelper.WriterType.JSON).ToArray();
        }


        /// <summary>
        /// Searches the core for specified query.
        /// </summary>
        /// <param name="query">The solr query without ?q= prefix. All other query related parameters like fq, wt are allowed.</param>
        /// <param name="getAll">Specifies whether to return all the rows or not.</param>
        /// <param name="startIndex">Start index of search results.</param>
        /// <param name="maxRows">The maximum number of rows to return in a call to solr.</param>
        /// <returns>An instance of <see cref="TypedConnector&lt;TCoreDocumentType&gt;"/> from the server.</returns>
        public IEnumerable<XmlNode> SearchXML(string query, bool getAll = true, long startIndex = 0, int maxRows = 100)
        {
            IEnumerable<XmlNode> results = ParallelResultsFetcher<XmlNode>(query, 0, maxRows, getAll, string.Empty,
                       (data) => { XmlDocument fragment = new XmlDocument(); fragment.InnerXml = data; return fragment.SelectNodes("//doc").Cast<XmlNode>().ToArray(); }
                       , HttpHelper.WriterType.XML);
            return results;
        }


        /// <summary>
        /// Returns a list of IDs from search results.
        /// </summary>
        /// <param name="query">Search query that returns documents with unique field. The return fields will be trimmed</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="startIndex">Start index of search results.</param>
        /// <param name="maxRows">The maximum number of rows to return in a call to solr.</param>
        /// <returns></returns>
        public string[] SearchIds(string query, long startIndex = 0, long maxRows = 100)
        {
            return ParallelResultsFetcher<string>(query, startIndex, maxRows, true, string.Empty, CsvListResponseFormatter, HttpHelper.WriterType.CSV).ToArray();
        }

        /// <summary>
        /// Saves the specified document to Solr core.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="commit">Specifies whether commit should be called after save.</param>
        /// <param name="optimize">Specifies whether optimize should be called after save.</param>
        public void Save(TCoreDocumentType document, bool commit = true, bool optimize = false)
        {
            SaveObject(document, commit, optimize);
        }

        /// <summary>
        /// Saves the specified documents to Solr core.
        /// </summary>
        /// <param name="documents">The documents.</param>
        /// <param name="commit">Specifies whether commit should be called after save.</param>
        /// <param name="optimize">Specifies whether optimize should be called after save.</param>
        public void Save(IList<TCoreDocumentType> documents, bool commit = true, bool optimize = false)
        {
            SaveObject(documents, commit, optimize);
        }

        /// <summary>
        /// Commits all pending changes to the core.
        /// </summary>
        public void Commit()
        {
            HttpHelper.HttpCommunicate(_updateUrl + "?commit=true");
        }

        /// <summary>
        /// Starts an optimize cycle on the core.
        /// </summary>
        public void Optimize()
        {
            HttpHelper.HttpCommunicate(_updateUrl + "?optimize=true");
        }

        /// <summary>
        /// Uses CSV to post a PK and FK kind of save to Solr core. New Id's will be added and missing Id's will be deleted. Any existing Id's won't be changed.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="bulkIds">The bulk ids.</param>
        /// <param name="bulkIdFieldName">Name of the bulk id field.</param>
        /// <param name="additionalFieldNames">The additional field names.</param>
        /// <param name="getAdditionalFieldValues">Function delegate to return additional fields.</param>
        /// <param name="commit">Specifies whether commit should be called after save.</param>
        /// <param name="optimize">Specifies whether optimize should be called after save.</param>
        public void BulkUpdate(string primaryKey, string[] bulkIds, string bulkIdFieldName, string[] additionalFieldNames = null, Func<string[]> getAdditionalFieldValues = null, bool commit = true, bool optimize = false)
        {
            string[] allIds = ParallelResultsFetcher<string>(string.Format("{0}:{1}", _uniqueFieldName, primaryKey), 0, 100, true, bulkIdFieldName, CsvListResponseFormatter, HttpHelper.WriterType.CSV).ToArray();
            string[] newIds = (from b in bulkIds where !allIds.Contains(b) select b).ToArray();
            string[] deletedIds = (from a in allIds where !bulkIds.Contains(a) select a).ToArray();

            BulkDelete(primaryKey, deletedIds, bulkIdFieldName);
            BulkAdd(primaryKey, newIds, bulkIdFieldName, additionalFieldNames, getAdditionalFieldValues);

            CommitAndOptimize(commit, optimize);
        }


        /// <summary>
        /// Add/replaces documents based on PK/FK combination
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="bulkIds">The bulk ids.</param>
        /// <param name="bulkIdFieldName">Name of the bulk id field.</param>
        /// <param name="additionalFieldNames">The additional field names.</param>
        /// <param name="getAdditionalFieldValues">Function delegate to return additional fields.</param>
        public void BulkAdd(string primaryKey, string[] bulkIds, string bulkIdFieldName, string[] additionalFieldNames, Func<string[]> getAdditionalFieldValues)
        {
            const string csvContentType = "application/csv";
            if (bulkIds != null && bulkIds.Length > 0)
            {
                Func<string[]> additionalFieldValuesFetcher = getAdditionalFieldValues ?? (() => { return new string[] { "" }; });
                List<string> fields = new List<string>(new[] { _uniqueFieldName, bulkIdFieldName });
                fields.AddRange(additionalFieldNames);
                string csvHeader = "fieldnames=" + string.Join(",", fields.ToArray());
                string updateUrl = _csvUpdateUrl + "?" + csvHeader;
                string addStream = string.Join(
                    "\n", (from b in bulkIds.Take(500) select string.Join(",", primaryKey, b, string.Join(",", additionalFieldValuesFetcher()))).ToArray());
                if (bulkIds.Length > 500)
                {
                    List<string> addStreams = new List<string>();
                    addStreams.Add(addStream);
                    for (int batch = 0, startRow = 500 + 1; startRow < bulkIds.LongLength; startRow += 500, batch++)
                    {
                        addStreams.Add(
                            string.Join(
                                "\n",
                                (from b in bulkIds.Skip(batch * 500).Take(500) select string.Format(",", primaryKey, b, additionalFieldValuesFetcher()).ToArray()))
                            );
                    }
                    addStreams.AsParallel().ForAll(a => HttpHelper.HttpCommunicate(updateUrl, a, csvContentType, post: true));
                }
                else
                {
                    HttpHelper.HttpCommunicate(updateUrl, addStream, csvContentType, post: true);
                }
            }
        }

        /// <summary>
        /// Deletes documents based on simple PK/FK combination. Uses a query to delete the documents.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="bulkIds">The bulk ids.</param>
        /// <param name="bulkIdFieldName">Name of the bulk id field.</param>
        public void BulkDelete(string primaryKey, string[] bulkIds, string bulkIdFieldName)
        {
            if (bulkIds != null && bulkIds.Length > 0)
            {
                const string deleteQueryFmt = "{{\"delete\":{{ \"query\":\"{0}:{1} AND ({2})\"}}}}";
                string[] clause = (from b in bulkIds.Skip(0).Take(Math.Min(500, bulkIds.Length)) select bulkIdFieldName + ":" + b).ToArray();
                string deleteQuery = string.Format(deleteQueryFmt, _uniqueFieldName, primaryKey, string.Join(" OR ", clause));
                //string deleteQuery = JsonConvert.SerializeObject(new { delete = new { query = string.Format("{0}:{1} and ({2})", _uniqueFieldName, primaryKey, string.Join(" or ", clause)) } });

                if (bulkIds.Length > 500)
                {
                    List<string> queries = new List<string>();
                    queries.Add(deleteQuery);
                    for (int batch = 0, startRow = 500 + 1; startRow < bulkIds.LongLength; startRow += 500, batch++)
                    {
                        queries.Add(
                            string.Format(deleteQueryFmt, _uniqueFieldName, primaryKey, string.Join(" OR ",
                            (from b in bulkIds.Skip(batch * 500).Take(500) select bulkIdFieldName + ":" + b).ToArray())));
                    }
                    queries.AsParallel().ForAll(q => HttpHelper.HttpCommunicate(_updateUrl, q, post: true));
                }
                else
                {
                    HttpHelper.HttpCommunicate(_updateUrl, deleteQuery, post: true);
                }
            }
        }
        #endregion
    }
}
