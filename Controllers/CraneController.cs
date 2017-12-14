using System;
using System.Linq;
using System.Web.Mvc;
using Intranet.Models;
using Intranet.Utils;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Intranet.Controllers
{
    public class CraneController : Controller
    {
        transportEntities _db = new transportEntities();
        //
        // GET: /Crane/

        public ActionResult Index()
        {
            if (User.IsInRole("LAN\\TR_Managers") || AccountManager.IsApprover(User.Identity.Name).Item2 || User.IsInRole("LAN\\TR_Viewers"))
            {
                var model = _db.RequestCranes
                    .Where(rc => (rc.Request.IsDeleted == false || rc.Request.IsDeleted == null))
                    .OrderByDescending(rc => rc.Request.PublishDate)
                    .ToList();

                return View(model);
            }
            else
            {
                var model = _db.RequestCranes
                    .Where(rc => ((rc.Request.IsDeleted == false || rc.Request.IsDeleted == null) && rc.Request.UserLogin == User.Identity.Name))
                    .OrderByDescending(rc => rc.Request.PublishDate)
                    .ToList();

                return View(model); 
            }
        }

        //
        // GET: /Crane/Details/5

        public ActionResult Details(int id)
        {
            var model = _db.RequestCranes.Single(rc => rc.RequestId == id);
            model.RequestCraneSlingers = model.RequestCraneSlingers.Where(rcs => rcs.IsDeleted == false).ToList();

            //if ((model.Request.UserLogin != User.Identity.Name) && !User.IsInRole("LAN\\TR_Managers"))
            //{
            //    ViewBag.BackController = "Crane";
            //    return View("Denied");
            //}

            //Показать детали  только автору, менеджеру, аппруверу, админу, либо вьюверу
            if (model.Request.UserLogin == User.Identity.Name || User.IsInRole("LAN\\TR_Managers") || AccountManager.IsApprover(User.Identity.Name).Item2 || User.IsInRole("LAN\\TR_Admins") || User.IsInRole("LAN\\TR_Viewers"))
            {
                return View(model);
            }
            else
            {
                ViewBag.BackController = "Home";
                return View("Denied"); 
            }
        }

        //
        // GET: /Crane/Create

        public ActionResult Create(int? id)
        {
            var rc = new RequestCrane();
            rc.RequestCraneSlingers.Add(new RequestCraneSlinger());

            if (id != null)
            {
                rc = _db.Requests.Single(r => r.RequestId == id).RequestCrane;
                rc.RequestCraneSlingers = rc.RequestCraneSlingers.Where(rcs => rcs.IsDeleted == false).ToList();
                rc.RequestCraneSlingers.ToList().ForEach(x =>
                {
                    x.SlingerId = 0;
                    x.RequestId = 0;
                });

                rc.RequestId = 0;

            }
            


            return View(rc);
        } 

        //
        // POST: /Crane/Create

        [HttpPost]
        public ActionResult Create(RequestCrane rc)
        {
            if (rc.Request.ApproverEmployeeId == 0)
            {
                ViewBag.ErrorMessage = "Веберите руководителя, который подпишет заявку!";
                return View(rc);
            }

            try
            {
                rc.Request.Status = 0;
                rc.Request.UserLogin = User.Identity.Name;
                rc.Request.UserFio = AccountManager.GetUserDisplayName(User.Identity.Name);
                rc.Request.PublishDate = DateTime.Now;
                rc.Request.IsDeleted = false;

                if (rc.Request.CustomerId == 0)
                    rc.Request.CustomerId = null;

                if (Utils.AccountManager.IsApprover(User.Identity.Name).Item2)
                {
                    rc.Request.SendToSpecTrans = true;
                    rc.Request.Status = 1;
                    rc.Request.ApproveDate = DateTime.Now;
                    rc.Request.ApproverLogin = User.Identity.Name;
                    rc.Request.ApproverFio = Utils.AccountManager.GetUserDisplayName(User.Identity.Name);

                    var re = new RequestEvent {Status = 1, EventDate = DateTime.Now};
                    rc.Request.RequestEvents.Add(re);
                }

                _db.RequestCranes.Add(rc);
                _db.SaveChanges();

                return View("Published", rc.Request.RequestId);
            }
            catch (DbEntityValidationException dbEx)
            {

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        //TODO: Попытка удалить запись, у которой ExpDate истек
                    }                    
                }

                ViewBag.ErrMessage = "Ошибка при создании записи";
                ViewBag.BackController = "Home";
                return View("Error");
            }  
        }

        public ActionResult AddSlingerRow(int id)
        {
            return PartialView("_SlingerAddRow", id);
        }

        
        //
        // GET: /Crane/Edit/5
 
        public ActionResult Edit(int id)
        {
            var model = _db.RequestCranes.Single(rc => rc.RequestId == id);
            model.RequestCraneSlingers = model.RequestCraneSlingers.Where(rcs => rcs.IsDeleted == false).ToList();

            //if ((model.Request.UserLogin != User.Identity.Name) && !User.IsInRole("LAN\\TR_Managers"))
            //{
            //    ViewBag.BackController = "Crane";
            //    return View("Denied");
            //}

            if (model.Request.SpecTransReceived != null && model.Request.SpecTransReceived.Value)
            {
                ViewBag.BackController = "Home";
                return View("Sended");
            }


            //редактировать только автору, менеджеру, админу
            if (model.Request.UserLogin == User.Identity.Name || User.IsInRole("LAN\\TR_Managers") || User.IsInRole("LAN\\TR_Admins"))
            {
                return View(model);
            }
            else
            {
                ViewBag.BackController = "Home";
                return View("Denied");
            }
        }

        //
        // POST: /Crane/Edit/5

        [HttpPost]
        public ActionResult Edit(RequestCrane rc)
        {
            try
            {
                var model = _db.RequestCranes.Single(m => m.RequestId == rc.RequestId);
                
                model.Request.RequestDate = rc.Request.RequestDate;
                model.LicenceNumber = rc.LicenceNumber;
                model.WorkPlace = rc.WorkPlace;
                model.WorkObject = rc.WorkObject;
                model.WorkType = rc.WorkType;
                model.CraneType = rc.CraneType;
                model.PowerLineExists = rc.PowerLineExists;
                model.PowerPermission = rc.PowerPermission;
                model.Responsible = rc.Responsible;
                model.ProjectExists = rc.ProjectExists;
                model.ResponsibleOrder = rc.ResponsibleOrder;
                model.CustomerName = rc.CustomerName;
                model.Request.ApproverEmployeeId = rc.Request.ApproverEmployeeId;

                if (rc.Request.CustomerId == 0)
                    rc.Request.CustomerId = null;
                model.Request.CustomerId = rc.Request.CustomerId;

                if (model.Request.Status == 3 && User.Identity.Name.ToLower() == model.Request.UserLogin.ToLower())
                {
                    model.Request.Status = 0;
                    RequestEvent re = new RequestEvent();
                    re.Status = 0;
                    re.Message = String.Empty;
                    re.EventDate = DateTime.Now;
                    model.Request.RequestEvents.Add(re);
                }

                var modelSlingers = model.RequestCraneSlingers;
                var respSlingers = rc.RequestCraneSlingers;

                foreach (var modelSlinger in modelSlingers)
                {
                    //Существует ли такой груз в респонсе
                    bool slingerExistInResponse = false;

                    foreach (var respSlinger in respSlingers)
                    {
                        //Если обновление груза
                        if (respSlinger.SlingerId == modelSlinger.SlingerId)
                        {
                            modelSlinger.Office = respSlinger.Office;
                            modelSlinger.FIO = respSlinger.FIO;
                            modelSlinger.CertificateNumber = respSlinger.CertificateNumber;
                            modelSlinger.DateKnowledge = respSlinger.DateKnowledge;                        

                            slingerExistInResponse = true;
                        }
                    }

                    if (slingerExistInResponse == false)
                        modelSlinger.IsDeleted = true;
                }

                //Добавляем все новые грузы, т.е те, у которых CargoId == 0
                foreach (var respSlinger in respSlingers)
                    if (respSlinger.SlingerId == 0)
                        modelSlingers.Add(respSlinger);

                _db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ViewBag.ErrMessage = "Ошибка при редактировании записи";
                ViewBag.BackController = "Home";
                return View("Error");
            }
        }

        //
        // GET: /Crane/Delete/5
 
        public ActionResult Delete(int id)
        {
            var model = _db.RequestCranes.Single(rc => rc.RequestId == id);

            //if ((model.Request.UserLogin != User.Identity.Name) && !User.IsInRole("LAN\\TR_Managers"))
            //{
            //    ViewBag.BackController = "Crane";
            //    return View("Denied");
            //} 
            //return View(model);


            if (model.Request.SpecTransReceived != null && model.Request.SpecTransReceived.Value)
            {
                ViewBag.BackController = "Home";
                return View("Sended");
            }


            //удалять только автору, менеджеру, админу
            if (model.Request.UserLogin == User.Identity.Name || User.IsInRole("LAN\\TR_Managers") || User.IsInRole("LAN\\TR_Admins"))
            {
                return View(model);
            }
            else
            {
                ViewBag.BackController = "Home";
                return View("Denied");
            }   
        }

        //
        // POST: /Crane/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var model = _db.RequestCranes.Single(rc => rc.RequestId == id);
                model.Request.IsDeleted = true;
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ViewBag.ErrMessage = "Ошибка при удалении записи";
                ViewBag.BackController = "Home";
                return View("Error");
            }
        }

        // GET: /Passenger/Process/5
        //[Authorize(Roles = "LAN\\TR_Managers")]
        public ActionResult Process(int id)
        {
            if (User.IsInRole("LAN\\TR_Managers") || Utils.AccountManager.IsApprover(User.Identity.Name).Item2)
            {
                var model = _db.RequestCranes.Single(rc => rc.RequestId == id);
                return View(model);
            }
            else
            {
                ViewBag.ErrMessage = "У текущего пользователя отсутствует право подписи заявок!";
                ViewBag.BackController = "Home";
                return View("Error"); 
            }
        }
     
    }
}
