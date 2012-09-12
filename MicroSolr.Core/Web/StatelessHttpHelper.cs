// -----------------------------------------------------------------------
// <copyright file="StatelessHttpHelper.cs" company="Imran Saeed">
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

namespace MicroSolr.Core.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net;
    using System.IO;

    /// <summary>
    /// A simple HTTP Get/Post class where each call creates new set of objects and is totally independant of other calls. 
    /// <remarks>This is a blocking call class.</remarks>
    /// </summary>
    public class StatelessHttpHelper : IHttpHelper
    {
        public string Get(Uri uri)
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadString(uri);
            }
        }

        public string Post(Uri uri, string content, string contentType, Encoding bytesConverter)
        {
            WebRequest request = HttpWebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = string.Format("{0};charset={1}", contentType, Encoding.UTF8.HeaderName);
            byte[] bytes = bytesConverter.GetBytes(content);
            request.ContentLength = bytes.Length;

            using (Stream writeStream = request.GetRequestStream())
            {
                string response = string.Empty;
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
                    return response;
                }
            }
        }
    }
}
