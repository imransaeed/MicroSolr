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

    /// <summary>
    /// A light weight class to query solr core
    /// </summary>
    public sealed class TypedConnector<TCoreDocumentType>
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
                    IList<string> queries = HttpHelper.MakeBatchSearchQueries(formattedQuery, startIndex, maxRows, responseObject.NumFound, writerType);
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


        //private string[] FetchBulkIds(string query, int startIndex, int maxRows, int batchSize, string returnFieldName)
        //{
        //    string fieldsOnlyQuery = query + "&fl=" + returnFieldName;
        //    JsonResponse responseObject = QueryCore<JsonResponse>(HttpHelper.MakeSearchQuery(fieldsOnlyQuery, HttpHelper.WriterType.JSON, startIndex, 0), JsonResponseFormatter);

        //    if (responseObject.NumFound > maxRows)
        //    {
        //        IList<string> queries = HttpHelper.MakeBatchSearchQueries(fieldsOnlyQuery, startIndex, maxRows, responseObject.NumFound, HttpHelper.WriterType.CSV);

        //        List<string> ids = new List<string>();
        //        queries.AsParallel().ForAll(q =>
        //        {
        //            string[] batchIds = QueryCore<string[]>(q, CsvListResponseFormatter);
        //            lock (ids)
        //            {
        //                ids.AddRange(batchIds);
        //            }
        //        });
        //    }
        //    else
        //    {
        //        return QueryCore<string[]>(HttpHelper.MakeSearchQuery(fieldsOnlyQuery, HttpHelper.WriterType.CSV), CsvListResponseFormatter);
        //    }
        //    return null;
        //}
        #endregion

        #region Public

        public const int DEFAULT_START_INDEX = 0;
        public const int DEFAULT_MAX_ROWS = 100;
        public const int DEFAULT_BATCH_SIZE = 500;

        public TypedConnector(string baseUrl, string uniqueFieldName = "")
        {
            _uniqueFieldName = uniqueFieldName;
            _baseUrl = new Uri(baseUrl.TrimEnd('/')).ToString(); // Normalizes the string
            _updateUrl = new Uri(_baseUrl + "/update/json/").ToString();
            _csvUpdateUrl = new Uri(_baseUrl + "/update/csv/").ToString();
            _queryUrl = new Uri(_baseUrl + "/select/").ToString();

        }

        public IList<TCoreDocumentType> Search(string query, bool getAll = true, long startIndex = DEFAULT_START_INDEX, int maxRows = DEFAULT_MAX_ROWS)
        {

            return ParallelResultsFetcher<TCoreDocumentType>(query, 0, maxRows, getAll, string.Empty,
                (data) => { return JsonResponseFormatter(data).Response.docs; }
                , HttpHelper.WriterType.JSON).ToArray();

            //List<TCoreDocumentType> documents = new List<TCoreDocumentType>();
            //JsonResponse responseObject = QueryCore<JsonResponse>(HttpHelper.MakeSearchQuery(query, HttpHelper.WriterType.JSON, startIndex, maxRows), JsonResponseFormatter);
            //documents.AddRange(responseObject.Response.docs);
            //if (maxRows < 0)
            //{
            //    maxRows = DEFAULT_MAX_ROWS;
            //}
            //if (responseObject.NumFound > maxRows )
            //{
            //    if (getAll)
            //    {
            //        IList<string> queries = HttpHelper.MakeBatchSearchQueries(query, startIndex, maxRows, responseObject.NumFound, HttpHelper.WriterType.JSON);

            //        queries.AsParallel().ForAll(q =>
            //        {
            //            JsonResponse batchResponseObject = QueryCore<JsonResponse>(q, JsonResponseFormatter);
            //            lock (documents)
            //            {
            //                documents.AddRange(batchResponseObject.Response.docs);
            //            }
            //        });
            //    }
            //    else
            //    {
            //        System.Diagnostics.Debug.WriteLine("Results are being ignored.");
            //    }
            //}

            //return documents;
        }


        public string[] SeachIds(string query, long startIndex = DEFAULT_START_INDEX, long maxRows = DEFAULT_MAX_ROWS)
        {
            //return FetchBulkIds(query, startIndex, maxRows, batchSize, _uniqueFieldName);
            return ParallelResultsFetcher<string>(query, startIndex, maxRows, true, string.Empty, CsvListResponseFormatter, HttpHelper.WriterType.CSV).ToArray();
        }


        private void SaveObject(object document, bool commit, bool optimize)
        {
            HttpHelper.HttpCommunicate(_updateUrl, JsonConvert.SerializeObject(document), post: true);
            CommitAndOptimize(commit, optimize);
        }

        public void Save(TCoreDocumentType document, bool commit = true, bool optimize = false)
        {
            SaveObject(document, commit, optimize);
        }

        public void Save(IList<TCoreDocumentType> documents, bool commit = true, bool optimize = false)
        {
            SaveObject(documents, commit, optimize);
        }

        public void Commit()
        {
            HttpHelper.HttpCommunicate(_updateUrl + "?commit=true");
        }

        public void Optimize()
        {
            HttpHelper.HttpCommunicate(_updateUrl + "?optimize=true");
        }

        // Uses CSV to submit large amounts of data
        public void BulkUpdate(string primaryKey, string[] bulkIds, string bulkIdFieldName, string[] additionalFieldNames = null, Func<string[]> getAdditionalFieldValues = null, bool commit = true, bool optimize = false)
        {
            string[] allIds = ParallelResultsFetcher<string>(string.Format("{0}:{1}", _uniqueFieldName, primaryKey), 0, DEFAULT_MAX_ROWS, true, bulkIdFieldName, CsvListResponseFormatter, HttpHelper.WriterType.CSV).ToArray();
            string[] newIds = (from b in bulkIds where !allIds.Contains(b) select b).ToArray();
            string[] deletedIds = (from a in allIds where !bulkIds.Contains(a) select a).ToArray();

            BulkDelete(primaryKey, deletedIds, bulkIdFieldName);
            BulkAdd(primaryKey, newIds, bulkIdFieldName, additionalFieldNames, getAdditionalFieldValues);

            CommitAndOptimize(commit, optimize);
        }


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
                    "\n", (from b in bulkIds.Take(DEFAULT_BATCH_SIZE) select string.Join(",", primaryKey, b, string.Join(",", additionalFieldValuesFetcher()))).ToArray());
                if (bulkIds.Length > DEFAULT_BATCH_SIZE)
                {
                    List<string> addStreams = new List<string>();
                    addStreams.Add(addStream);
                    for (int batch = 0, startRow = DEFAULT_BATCH_SIZE + 1; startRow < bulkIds.LongLength; startRow += DEFAULT_BATCH_SIZE, batch++)
                    {
                        addStreams.Add(
                            string.Join(
                                "\n",
                                (from b in bulkIds.Skip(batch * DEFAULT_BATCH_SIZE).Take(DEFAULT_BATCH_SIZE) select string.Format(",", primaryKey, b, additionalFieldValuesFetcher()).ToArray()))
                            );

                        addStreams.AsParallel().ForAll(a => HttpHelper.HttpCommunicate(updateUrl, a, csvContentType, post: true));
                    }
                }
                else
                {
                    HttpHelper.HttpCommunicate(updateUrl, addStream, csvContentType, post: true);
                }
            }
        }

        public void BulkDelete(string primaryKey, string[] bulkIds, string bulkIdFieldName)
        {
            if (bulkIds != null && bulkIds.Length > 0)
            {
                const string deleteQueryFmt = "{{\"delete\":{{ \"query\":\"{0}:{1} AND ({2})\"}}}}";
                string[] clause = (from b in bulkIds.Skip(0).Take(Math.Min(DEFAULT_BATCH_SIZE, bulkIds.Length)) select bulkIdFieldName + ":" + b).ToArray();
                string deleteQuery = string.Format(deleteQueryFmt, _uniqueFieldName, primaryKey, string.Join(" OR ", clause));
                //string deleteQuery = JsonConvert.SerializeObject(new { delete = new { query = string.Format("{0}:{1} and ({2})", _uniqueFieldName, primaryKey, string.Join(" or ", clause)) } });

                if (bulkIds.Length > DEFAULT_BATCH_SIZE)
                {
                    List<string> queries = new List<string>();
                    queries.Add(deleteQuery);
                    for (int batch = 0, startRow = DEFAULT_BATCH_SIZE + 1; startRow < bulkIds.LongLength; startRow += DEFAULT_BATCH_SIZE, batch++)
                    {
                        queries.Add(
                            string.Format(deleteQueryFmt, _uniqueFieldName, primaryKey, string.Join(" OR ",
                            (from b in bulkIds.Skip(batch * DEFAULT_BATCH_SIZE).Take(DEFAULT_BATCH_SIZE) select bulkIdFieldName + ":" + b).ToArray())));
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
