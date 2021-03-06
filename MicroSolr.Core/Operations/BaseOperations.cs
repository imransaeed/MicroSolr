﻿// -----------------------------------------------------------------------
// <copyright file="BaseOperations.cs" company="Imran Saeed">
// Copyright (c) 2012 Imran Saeed
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Core.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using MicroSolr.Core.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class BaseOperations : IOperations
    {
        protected IHttpHelper _httpHelper;

        public abstract IEnumerable<TOutput> Load<TOutput>(ILoadCommand command, IDataSerializer<TOutput> serializer, IResponseFormatter<string> formatter);

        public abstract IOperations Save<TData>(ISaveCommand<TData> command, IDataSerializer<TData> serializer, bool commit = true, bool optimize = false);

        public BaseOperations(Uri baseUri, string coreName, IHttpHelper httpHelper)
        {
            _httpHelper = httpHelper ?? new StatelessHttpHelper();
            CoreUri = new Uri(string.Format("{0}/{1}/", baseUri.ToString().TrimEnd('/'), coreName));
            SelectUri = new Uri(CoreUri.ToString() + "select/");
            UpdateUri = new Uri(CoreUri.ToString() + "update/");
        }

        public virtual IOperations Commit()
        {
            Uri u = MakeUri(UpdateUri, "commit=true");
            _httpHelper.Get(u);
            return this;
        }

        public virtual IOperations Optimize()
        {
            Uri u = MakeUri(UpdateUri, "optimize=true");
            _httpHelper.Get(u);
            return this;
        }

        protected Uri CoreUri
        {
            get;
            private set;
        }

        protected Uri SelectUri
        {
            get;
            private set;
        }

        protected Uri UpdateUri
        {
            get;
            private set;
        }

        protected long GetRowCountForResults(ILoadCommand command)
        {
            string qs = MakeRowCountQueryString(command);
            Uri rowCountUri = MakeUri(SelectUri, qs);
            string results = _httpHelper.Get(rowCountUri);
            XDocument resultsDoc = XDocument.Parse(results);
            var rootNode = (from r in resultsDoc.Descendants() where r.Attribute("name").Name == "response" select r).First();
            long rows = 0;
            var val = rootNode.Attribute("numFound").Value;
            long.TryParse(val, out rows);

            return rows;
        }

        protected IEnumerable<TOutput> ExecuteLoad<TOutput>(string loadQS, FormatType responseFormat, IDataSerializer<TOutput> serializer, IResponseFormatter<string> formatter)
        {
            Uri loadUri = MakeUri(SelectUri, loadQS);
            string response = _httpHelper.Get(loadUri);
            string formattedResponse = formatter != null ? formatter.Format(response) : response;
            return serializer.DeSerialize(formattedResponse, responseFormat);
        }

        protected IOperations ExecuteSave<TData>(IEnumerable<TData> data, IDataSerializer<TData> serializer, bool commit, bool optimize)
        {
            _httpHelper.Post(UpdateUri, serializer.Serialize(data, FormatType.JSON), "application/json", Encoding.UTF8);
            if (commit) Commit();
            if (optimize) Optimize();
            return this;
        }

        protected static Uri MakeUri(Uri baseUri, string queryString)
        {
            UriBuilder builder = new UriBuilder(baseUri);
            builder.Query = queryString;
            return builder.Uri;
        }

        protected static string MakeLoadQueryString(ILoadCommand command)
        {
            IDictionary<string, string> qsParts = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(command.Query))
            {
                qsParts.Add("q", command.Query);
            }

            qsParts.Add("wt", Enum.GetName(typeof(FormatType), command.ResponseFormat).ToLowerInvariant());
            return QueryStringFromDicionary(qsParts);
        }

        protected static string MakeRowCountQueryString(ILoadCommand command)
        {
            IDictionary<string, string> qsParts = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(command.Query))
            {
                qsParts.Add("q", command.Query);
            }
            qsParts.Add("rows", "0");
            qsParts.Add("wt", "XML");

            return QueryStringFromDicionary(qsParts);
        }

        private static string QueryStringFromDicionary(IDictionary<string, string> qsParts)
        {
            var qList = from k in qsParts.Keys select string.Format("{0}={1}", k, qsParts[k]);

            return string.Join("&", qList);
        }
    }
}
