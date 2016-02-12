using Intranet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Intranet.Utils;
using Intranet.ViewModels;

namespace Intranet.Controllers
{
    public class HomeController : Controller
    {

        transportEntities _db = new transportEntities();

        public ActionResult Index(int? page)
        {
            var indexList = new IndexList();
            indexList.RowPerPage = 30;
            int skip = 0;
            if (page.HasValue)
            {
                skip = indexList.RowPerPage * ((Int32)page - 1);
            }

            //Общий список заявок, доступен TR_Managers, Viewers и Тем, кто имеет право подписи
            if (User.IsInRole("LAN\\TR_Managers") || AccountManager.IsApprover(User.Identity.Name).Item2 || User.IsInRole("LAN\\TR_Viewers"))
            {                
                var model = _db.Requests
                    .Where(r => (r.IsDeleted != true || r.IsDeleted == null))
                    .OrderByDescending(r => r.PublishDate)
                    .Skip(skip)
                    .Take(30)
                    .ToList();

                foreach (var r in model)
                {
                    r.DetailsLink = RequestType.GetDetailsLink(r);
                    r.DetailsButton = RequestType.GetDetailsButton(r);
                    r.RequestTypeLabel = RequestType.GetRequestTypeLabel(r);
                }                
                indexList.Requests = model;
                indexList.TotalRows = _db.Requests.Count(r => (r.IsDeleted != true || r.IsDeleted == null));
                return View(indexList);
            }
            else //Список заявок конкретного пользователя
            {
                var model = _db.Requests
                    .Where(r => ((r.IsDeleted != true || r.IsDeleted == null) && r.UserLogin == User.Identity.Name))
                    .OrderByDescending(r => r.PublishDate)
                    .Skip(skip)
                    .Take(30)
                    .ToList();

                foreach (var r in model)
                {
                    r.DetailsLink = RequestType.GetDetailsLink(r);
                    r.DetailsButton = RequestType.GetDetailsButton(r);
                    r.RequestTypeLabel = RequestType.GetRequestTypeLabel(r);
                }
                indexList.Requests = model;
                indexList.TotalRows = _db.Requests.Count(r => ((r.IsDeleted != true || r.IsDeleted == null) && r.UserLogin == User.Identity.Name));
                return View(indexList); 
            }
        }

        public ActionResult About()
        {
            return View();
        }

        public PartialViewResult FilterRequests(string rDate)
        {
            IndexList indexList = new IndexList();
            DateTime requestDate;
            if (DateTime.TryParseExact(rDate, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out requestDate))
            {
                if (User.IsInRole("LAN\\TR_Managers") || AccountManager.IsApprover(User.Identity.Name).Item2 || User.IsInRole("LAN\\TR_Viewers"))
                {
                    var model = _db.Requests
                        .Where(r => ((r.IsDeleted != true || r.IsDeleted == null) && r.RequestDate == requestDate))
                        .OrderByDescending(r => r.PublishDate)
                        .ToList();
                    foreach (var r in model)
                    {
                        r.DetailsLink = RequestType.GetDetailsLink(r);
                        r.DetailsButton = RequestType.GetDetailsButton(r);
                        r.RequestTypeLabel = RequestType.GetRequestTypeLabel(r);
                    }
                    indexList.Requests = model;
                    indexList.TotalRows = model.Count;
                    indexList.RowPerPage = model.Count;
                    return PartialView("_Table", indexList);
                }
                else
                {
                    var model = _db.Requests
                        .Where(r => ((r.IsDeleted != true || r.IsDeleted == null) && r.UserLogin == User.Identity.Name && r.RequestDate == requestDate))
                        .OrderByDescending(r => r.PublishDate)
                        .ToList();
                    foreach (var r in model)
                    {
                        r.DetailsLink = RequestType.GetDetailsLink(r);
                        r.DetailsButton = RequestType.GetDetailsButton(r);
                        r.RequestTypeLabel = RequestType.GetRequestTypeLabel(r);
                    }
                    indexList.Requests = model;
                    indexList.TotalRows = model.Count;
                    indexList.RowPerPage = model.Count;
                    return PartialView("_Table", indexList);
                }              
            }
            else
            {
                if (User.IsInRole("LAN\\TR_Managers") || AccountManager.IsApprover(User.Identity.Name).Item2 || User.IsInRole("LAN\\TR_Viewers"))
                {
                    var model = _db.Requests
                        .Where(r => (r.IsDeleted != true || r.IsDeleted == null))
                        .OrderByDescending(r => r.PublishDate)
                        .ToList();

                    foreach (var r in model)
                    {
                        r.DetailsLink = RequestType.GetDetailsLink(r);
                        r.DetailsButton = RequestType.GetDetailsButton(r);
                        r.RequestTypeLabel = RequestType.GetRequestTypeLabel(r);
                    }
                    indexList.Requests = model;
                    indexList.TotalRows = model.Count;
                    indexList.RowPerPage = model.Count;
                    return PartialView("_Table", indexList);
                }
                else
                {
                    var model = _db.Requests
                        .Where(r => ((r.IsDeleted != true || r.IsDeleted == null) && r.UserLogin == User.Identity.Name))
                        .OrderByDescending(r => r.PublishDate)
                        .ToList();

                    foreach (var r in model)
                    {
                        r.DetailsLink = RequestType.GetDetailsLink(r);
                        r.DetailsButton = RequestType.GetDetailsButton(r);
                        r.RequestTypeLabel = RequestType.GetRequestTypeLabel(r);
                    }
                    indexList.Requests = model;
                    indexList.TotalRows = model.Count;
                    indexList.RowPerPage = model.Count;
                    return PartialView("_Table", indexList);
                }            
            }
        }

        public ActionResult RequestsToApprove(int id)
        {
           IndexList indexList = new IndexList();
           if (!_db.RequestApprovers.Any( a => a.EmployeeId == id) && !User.IsInRole("LAN\\TR_Admins"))
           {
              ViewBag.BackController = "Home";
              return View("Denied");
           }

           var model = _db.Requests
                    .Where(r => ((r.IsDeleted != true || r.IsDeleted == null) && r.ApproverEmployeeId == id && r.Status == 0))
                    .OrderByDescending(r => r.PublishDate)
                    .ToList();

           foreach (var r in model)
           {
               r.DetailsLink = RequestType.GetDetailsLink(r);
               r.DetailsButton = RequestType.GetDetailsButton(r);
               r.RequestTypeLabel = RequestType.GetRequestTypeLabel(r);
           }
           indexList.Requests = model;
           indexList.TotalRows = model.Count;
           indexList.RowPerPage = model.Count;
           return View(indexList);
        }

        public PartialViewResult ShowAllRequestsToApprover(int id)
        {
            IndexList indexList = new IndexList();            

            var model = _db.Requests
                    .Where(r => ((r.IsDeleted != true || r.IsDeleted == null) && r.ApproverEmployeeId == id))
                    .OrderByDescending(r => r.PublishDate)
                    .ToList();

            foreach (var r in model)
            {
                r.DetailsLink = RequestType.GetDetailsLink(r);
                r.DetailsButton = RequestType.GetDetailsButton(r);
                r.RequestTypeLabel = RequestType.GetRequestTypeLabel(r);
            }
            ViewBag.Message = "Показаны подписанные и неподписанные";
            ViewBag.LinkText = "Показать только неподписанные";
            ViewBag.ActionName = "ShowNewRequestsToApprover";

            indexList.Requests = model;
            indexList.TotalRows = model.Count;
            indexList.RowPerPage = model.Count;
            return PartialView("_SignRequests", indexList);
        }

        public PartialViewResult ShowNewRequestsToApprover(int id)
        {
            IndexList indexList = new IndexList();

            var model = _db.Requests
                    .Where(r => ((r.IsDeleted != true || r.IsDeleted == null) && r.ApproverEmployeeId == id && r.Status == 0))
                    .OrderByDescending(r => r.PublishDate)
                    .ToList();

            foreach (var r in model)
            {
                r.DetailsLink = RequestType.GetDetailsLink(r);
                r.DetailsButton = RequestType.GetDetailsButton(r);
                r.RequestTypeLabel = RequestType.GetRequestTypeLabel(r);
            }
            ViewBag.Message = "Показаны неподписанные";
            ViewBag.LinkText = "Показать подписанные и неподписанные";
            ViewBag.ActionName = "ShowAllRequestsToApprover";

            indexList.Requests = model;
            indexList.TotalRows = model.Count;
            indexList.RowPerPage = model.Count;
            return PartialView("_SignRequests", indexList);
        }

        public ActionResult SignAll(int id)
        {
            if (!_db.RequestApprovers.Any(a => a.EmployeeId == id) && !User.IsInRole("LAN\\TR_Admins"))
            {
                ViewBag.BackController = "Home";
                return View("Denied");
            }

            try
            {
                var rqsts = _db.Requests.Where(rq => (rq.ApproverEmployeeId == id && (rq.Status == 0 || rq.Status == 3))).ToList();
                foreach (var rqst in rqsts)
                {
                    rqst.ApproveDate = DateTime.Now;
                    rqst.ApproverLogin = User.Identity.Name;
                    rqst.ApproverFio = AccountManager.GetUserDisplayName(User.Identity.Name);
                    rqst.Status = 1;

                    RequestEvent re = new RequestEvent();
                    re.Status = 1;
                    re.Message = String.Empty;
                    re.EventDate = DateTime.Now;

                    rqst.RequestEvents.Add(re);
                }
                _db.SaveChanges();
                ViewBag.Message = "Заявки подписаны!";                
            }
            catch
            {
                ViewBag.Message = "Ошибка при подписи заявок, заявки не подписаны!";                                                
            }
            return View("Signed");
        }
       

        [Authorize(Roles = "LAN\\TR_Managers")]
        [HttpPost]
        [HttpParamAction]
        public ActionResult Accept(string message, int RequestId)
        {
            try
            {
                var model = _db.Requests.Single(r => r.RequestId == RequestId);//_db.RequestFreights.Single(rf => rf.RequestId == RequestId);
                model.Status = 2;

                RequestEvent re = new RequestEvent();
                re.Status = 2;
                re.Message = message;
                re.EventDate = DateTime.Now;

                model.RequestEvents.Add(re);

                _db.SaveChanges();
                ViewBag.Message = "Заявка принята!";                                
            }
            catch
            {
                ViewBag.ErrMessage = @"Ошибка при ПРИНЯТИИ заявки";
                ViewBag.BackController = "Home";
                return View("Error");
            }
            return View("Signed");
        }


        [HttpPost]
        [HttpParamAction]
        public ActionResult Sign(string message, int RequestId)
        {
            if (User.IsInRole("TR_Admins") || AccountManager.IsApprover(User.Identity.Name).Item2)
            {
                try
                {
                    var model = _db.Requests.Single(r => r.RequestId == RequestId);
                    model.Status = 1;
                    model.ApproveDate = DateTime.Now;
                    model.ApproverLogin = User.Identity.Name;
                    model.ApproverFio = AccountManager.GetUserDisplayName(User.Identity.Name);
                    

                    RequestEvent re = new RequestEvent();
                    re.Status = 1;
                    re.Message = message;
                    re.EventDate = DateTime.Now;

                    model.RequestEvents.Add(re);

                    _db.SaveChanges();
                    ViewBag.Message = "Заявка подписана!";
                    //return RedirectToAction("Details", new { id = RequestId });
                }
                catch
                {
                    ViewBag.ErrMessage = @"Ошибка при ПОДПИСИ заявки";
                    ViewBag.BackController = "Home";
                    return View("Error");
                }
                return View("Signed");
            }
            else
            {
                ViewBag.ErrMessage = "У текущего пользователя отсутствует право подписи заявок!";
                ViewBag.BackController = "Home";
                return View("Error");
            }
        }

        //[Authorize(Roles = "LAN\\TR_Managers")]
        [HttpPost]
        [HttpParamAction]
        public ActionResult Reject(string message, int RequestId)
        {
            if (User.IsInRole("TR_Managers") || AccountManager.IsApprover(User.Identity.Name).Item2)
            {
                try
                {
                    var model = _db.Requests.Single(r => r.RequestId == RequestId);
                    model.Status = 3;

                    RequestEvent re = new RequestEvent();
                    re.Status = 3;
                    re.Message = message;
                    re.EventDate = DateTime.Now;

                    model.RequestEvents.Add(re);

                    _db.SaveChanges();
                    ViewBag.Message = "Заявка отклонена!";
                    //return RedirectToAction("Details", new { id = RequestId });
                }
                catch
                {
                    ViewBag.ErrMessage = @"Ошибка при ОТКЛОНЕИИ заявки";
                    ViewBag.BackController = "Home";
                    return View("Error");
                }
                return View("Signed");
            }
            else
            {
                ViewBag.ErrMessage = "У текущего пользователя отсутствует право подписи заявок!";
                ViewBag.BackController = "Home";
                return View("Error");
            }
        }

        public ActionResult Test(int id)
        {
            return View("Published", id);
        }
    }
}
