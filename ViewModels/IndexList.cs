using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.ViewModels
{
    public class IndexList
    {
        public IEnumerable<Intranet.Models.Request> Requests { get; set; }
        public int TotalRows { get; set; }
        public int RowPerPage { get; set; }
    }
}