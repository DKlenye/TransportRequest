using System.Collections.Generic;

namespace Intranet.ViewModels
{
    public class InternationalIndexList
    {
        public IEnumerable<Intranet.Models.RequestInternational> Requests { get; set; }
        public int TotalRows { get; set; }
        public int RowPerPage { get; set; }
    }
}