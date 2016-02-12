using Intranet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.Utils
{
    public class RequstsToSign
    {
        public static string RequstsToSignCount(int empId)
        {
            transportEntities _db = new transportEntities();
            var rqsts = _db.Requests.Where(rq => (rq.ApproverEmployeeId == empId && (rq.Status == 0) && (rq.IsDeleted != true))).ToList();
            return rqsts.Count.ToString();
        }
    }
}