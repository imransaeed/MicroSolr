// -----------------------------------------------------------------------
// <copyright file="LoadCommand.cs" company="Imran Saeed">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LoadCommand : ILoadCommand
    {
        public FormatType ResponseFormat 
        {
            get;
            set;
        }

        public long StartIndex
        {
            get;
            set;
        }

        public long MaxRows
        {
            get;
            set;
        }

        public bool GetAll
        {
            get;
            set;
        }

        public string Query
        {
            get;
            set;
        }

        public string FilterQuery
        {
            get;
            set;
        }

        public string FieldFilter
        {
            get;
            set;
        }

        public object Clone()
        {
            return new LoadCommand { FieldFilter = this.FieldFilter, Query = this.Query, FilterQuery = this.FilterQuery, GetAll = this.GetAll, MaxRows = this.MaxRows, ResponseFormat = this.ResponseFormat, StartIndex = this.StartIndex };
        }
    }
}
