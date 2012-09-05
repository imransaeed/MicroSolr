// -----------------------------------------------------------------------
// <copyright file="HttpHelper.cs" company="EF">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Connectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal static class HttpHelper
    {
        /// <summary>
        /// Simple post/get HTTP communicator.
        /// <remarks>All exceptions are thrown to the caller.</remarks>
        /// </summary>
        /// <param name="url">Fully qualified URL</param>
        /// <param name="content">Used for POSTing</param>
        /// <param name="contentType">Type of content when used for POSTing</param>
        /// <param name="bytesConverter">Bytes encoder for POSTing</param>
        /// <param name="post">Whether the data should be POSTed or GETed</param>
        /// <returns>Response stream as a string using the defaule response encoder</returns>
        public static string HttpCommunicate(string url, string content = "", string contentType = "application/json", Encoding bytesConverter = null, bool post = false)
        {
            string response = string.Empty;
            //UriBuilder requestUriBuilder = new UriBuilder(new Uri(url));

            Encoding streamEncoder = bytesConverter ?? UTF8Encoding.UTF8;
            if (post)
            {
                WebRequest request = HttpWebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = string.Format("{0};charset={1}", contentType, Encoding.UTF8.HeaderName);
                byte[] bytes = streamEncoder.GetBytes(content);
                request.ContentLength = bytes.Length;

                using (Stream writeStream = request.GetRequestStream())
                {
                    writeStream.Write(bytes, 0, bytes.Length);
                    writeStream.Flush();
                    writeStream.Close();
                    using (WebResponse webResponse = request.GetResponse())
                    {
                        using (Stream responseStream = webResponse.GetResponseStream())
                        {
                            using (StreamReader responseReader = new StreamReader(responseStream))
                            {
                                response = responseReader.ReadToEnd();
                            }
                        }
                        webResponse.Close();
                    }
                }
            }
            else
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.Encoding = Encoding.UTF8;
                    response = webClient.DownloadString(url);
                }
            }
            return response;
        }

        public enum WriterType
        {
            XML = 0,
            JSON = 1,
            CSV = 2
        }

        public static string MakeSearchQuery(string q, WriterType writerType = WriterType.JSON, long startIndex = 0, long maxRows = 100)
        {
            return string.Format("q={0}&wt={1}&start={2}&rows={3}", q, Enum.GetName(typeof(WriterType), writerType).ToLowerInvariant(), startIndex, maxRows);
        }

        public static IList<string> MakeBatchSearchQueries(string query, long startIndex, long maxRows, long totalRecords, WriterType writerType = WriterType.JSON)
        {
            List<string> queries = new List<string>();
            for (long batchStartIndex = startIndex + maxRows + 1; batchStartIndex < totalRecords; batchStartIndex += maxRows)
            {
                queries.Add(MakeSearchQuery(query, writerType, batchStartIndex, maxRows));
            }
            return queries;
        }
    }
}
