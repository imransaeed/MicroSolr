// -----------------------------------------------------------------------
// <copyright file="JsonSerializer.cs" company="EF">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Core.Serializers
{
    using System;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MultiFormatSerializer<TData> : IDataSerializer<TData>
    {
        #region JSonresponsecontainer
        private class Response
        {
            [JsonProperty("numFound")]
            public long NumFound { get; set; }

            [JsonProperty("start")]
            public long Start { get; set; }

            [JsonProperty("docs")]
            public TData[] Docs { get; set; }
        }

        private class JsonResponse
        {
            [JsonProperty("response")]
            public Response Response { get; set; }
        }
        #endregion

        public string Serialize(TData data, FormatType format)
        {
            switch (format)
            {
                case FormatType.XML:
                    throw new NotImplementedException("Feature not available yet");
                case FormatType.JSON:
                    return JsonConvert.SerializeObject(data);
                case FormatType.CSV:
                    throw new NotImplementedException("Feature not available yet");
                case FormatType.Custom:
                default:
                    throw new NotImplementedException("Please inherit custom serializer logic.");
            }
        }

        public string Serialize(IEnumerable<TData> data, FormatType format)
        {
            switch (format)
            {
                case FormatType.XML:
                    throw new NotImplementedException("Feature not available yet");
                case FormatType.JSON:
                    return JsonConvert.SerializeObject(data);
                case FormatType.CSV:
                    throw new NotImplementedException("Feature not available yet");
                case FormatType.Custom:
                default:
                    throw new NotImplementedException("Please inherit custom serializer logic.");
            }
        }

        public IEnumerable<TData> DeSerialize(string stream, FormatType format)
        {
            switch (format)
            {
                case FormatType.XML:
                    throw new NotImplementedException("Feature not available yet");
                case FormatType.JSON:
                    JsonResponse response = JsonConvert.DeserializeObject<JsonResponse>(stream);
                    return response.Response.Docs;
                case FormatType.CSV:
                    throw new NotImplementedException("Feature not available yet");
                case FormatType.Custom:
                default:
                    throw new NotImplementedException("Please inherit custom serializer logic.");
            }
            
        }
    }
}
