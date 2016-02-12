using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Intranet.Models;

namespace Intranet.ViewModels
{
    public class RequestPassengerViewModel
    {
        public RequestPassenger Rp { get; set; }
        public Dictionary<int, string> Approvers { get; set; }

        public RequestPassengerViewModel()
        {
            transportEntities _db = new transportEntities();
        }
    }
}