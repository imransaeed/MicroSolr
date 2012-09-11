// -----------------------------------------------------------------------
// <copyright file="HttpHelpers.cs" company="Imran Saeed">
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
    /// TODO: Update summary.
    /// </summary>
    public interface IHttpHelper
    {
        //string HttpCommunicate(Uri uri, string content, string contentType, Encoding bytesConverter, bool post);
        string Get(Uri uri);
        string Post(Uri uri, string content, string contentType, Encoding bytesConverter);
    }
}
