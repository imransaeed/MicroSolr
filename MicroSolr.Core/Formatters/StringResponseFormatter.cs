// -----------------------------------------------------------------------
// <copyright file="StringResponseFormatter.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Core.Formatters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StringResponseFormatter : IResponseFormatter<string>
    {
        public string Format(string data)
        {
            return data;
        }
    }
}
