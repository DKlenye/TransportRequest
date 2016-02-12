using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Intranet.Models;
using Intranet.Utils;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Intranet.Controllers
{
    public class PassengerController : Controller
    {
        transportEntities _db = new transportEntities();

        //
        // GET: /Passenger/

        public ActionResult Index()
        {
            if (User.IsInRole("LAN\\TR_Managers") || AccountManager.IsApprover(User.Identity.Name).Item2 || User.IsInRole("LAN\\TR_Viewers"))
            {
                var model = _db.RequestPassengers
                    .Where(rp => (rp.Request.IsDeleted != true || rp.Request.IsDeleted == null))
                    .OrderByDescending(rp => rp.Request.PublishDate)
                    .ToList();
                return View(model);
            }
            else
            {
                var model = _db.RequestPassengers
                    .Where(rp => ((rp.Request.IsDeleted != true || rp.Request.IsDeleted == null) && rp.Request.UserLogin == User.Identity.Name))
                    .OrderByDescending(rp => rp.Request.PublishDate)
                    .ToList();
                return View(model); 
            }
        }

        //
        // GET: /Passenger/Details/5

        public ActionResult Details(int id)
        {
            var model = _db.RequestPassengers.Single(pr => pr.RequestId == id);

            ////Если заявка не текущего пользователя и он не админ, то вернуть к списку
            //if ((model.Request.UserLogin != User.Identity.Name) && !User.IsInRole("LAN\\TR_Managers"))
            //{
            //    ViewBag.BackController = "Passenger";
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
        // GET: /Passenger/Create

        public ActionResult Create()
        {            
            return View(new RequestPassenger());
        } 

        //
        // POST: /Passenger/Create

        [HttpPost]
        public ActionResult Create(RequestPassenger rp)
        {
            if (ModelState.IsValid)
            {
                if (rp.Request.ApproverEmployeeId == 0)
                {
                    ViewBag.ErrorMessage = "Веберите руководителя, который подпишет заявку!";
                    return View(rp);
                }
                try
                {
                    
                    rp.Request.Status = 0;
                    rp.Request.UserLogin = User.Identity.Name;
                    rp.Request.UserFio = AccountManager.GetUserDisplayName(User.Identity.Name);
                    rp.Request.PublishDate = DateTime.Now;
                    rp.Request.IsDeleted = false;

                    if (Utils.AccountManager.IsApprover(User.Identity.Name).Item2)
                    {
                        rp.Request.Status = 1;
                        rp.Request.ApproveDate = DateTime.Now;
                        rp.Request.ApproverLogin = User.Identity.Name;
                        rp.Request.ApproverFio = Utils.AccountManager.GetUserDisplayName(User.Identity.Name);
                       
                        RequestEvent re = new RequestEvent();
                        re.Status = 1;                        
                        re.EventDate = DateTime.Now;
                        rp.Request.RequestEvents.Add(re);
                    }

                    if (rp.Request.CustomerId == 0)
                        rp.Request.CustomerId = null;

                    _db.RequestPassengers.Add(rp);
                    _db.SaveChanges();                    
                    return View("Published", rp.Request.RequestId);
                }
                catch
                {
                    ViewBag.ErrMessage = "Ошибка при создании записи";
                    ViewBag.BackController = "Passenger";
                    return View("Error");                   
                }
            }
            return View();
        }
        
        //
        // GET: /Passenger/Edit/5
 
        public ActionResult Edit(int id)
        {
            var model = _db.RequestPassengers.Single(rp => rp.RequestId == id);

            //if ((model.Request.UserLogin != User.Identity.Name) && !User.IsInRole("LAN\\TR_Managers"))
            //{
            //    ViewBag.BackController = "Passenger";
            //    return View("Denied");
            //}

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
        // POST: /Passenger/Edit/5

        [HttpPost]
        public ActionResult Edit(RequestPassenger rp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var model = _db.RequestPassengers.Single(m => m.RequestId == rp.RequestId);

                    model.DestinationPoint = rp.DestinationPoint;
                    model.PassengerAmount = rp.PassengerAmount;
                    model.ChildAmount = rp.ChildAmount;
                    model.Request.RequestDate = rp.Request.RequestDate;
                    model.OrderDate = rp.OrderDate;
                    model.OrderName = rp.OrderName;
                    model.OrderNumber = rp.OrderNumber;
                    model.TripPurpose = rp.TripPurpose;
                    model.TripDuration = rp.TripDuration;
                    model.SeatPlace = rp.SeatPlace;
                    model.SecondedPeople = rp.SecondedPeople;
                    model.Request.ApproverEmployeeId = rp.Request.ApproverEmployeeId;

                    if (rp.Request.CustomerId == 0)
                        rp.Request.CustomerId = null;

                    model.Request.CustomerId = rp.Request.CustomerId;

                    if (model.Request.Status == 3 && User.Identity.Name.ToLower() == model.Request.UserLogin.ToLower())
                    {
                        model.Request.Status = 0;
                        RequestEvent re = new RequestEvent();                        
                        re.Status = 0;
                        re.Message = String.Empty;
                        re.EventDate = DateTime.Now;
                        model.Request.RequestEvents.Add(re);
                    }

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
            return View();
        }

        //
        // GET: /Passenger/Delete/5
 
        public ActionResult Delete(int id)
        {
            var model = _db.RequestPassengers.Single(rp => rp.RequestId == id);

            //if ((model.Request.UserLogin != User.Identity.Name) && !User.IsInRole("LAN\\TR_Managers"))
            //{
            //    ViewBag.BackController = "Passenger";
            //    return View("Denied");
            //}

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
        // POST: /Passenger/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {                
                var model = _db.RequestPassengers.Single(rp => rp.RequestId == id);
                model.Request.IsDeleted = true;
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
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
                ViewBag.ErrMessage = "Ошибка при удалении записи";
                ViewBag.BackController = "Passenger"; 
                return View("Error");
            }
        }
        
        // GET: /Passenger/Process/5
        //[Authorize(Roles = "LAN\\TR_Managers")]
        public ActionResult Process(int id)
        {
            if (User.IsInRole("LAN\\TR_Managers") || Utils.AccountManager.IsApprover(User.Identity.Name).Item2)
            {
                var model = _db.RequestPassengers.Single(pr => pr.RequestId == id);
                return View(model);
            }
            else
            {
                ViewBag.ErrMessage = "У текущего пользователя отсутствует право подаписи заявок!";
                ViewBag.BackController = "Home";
                return View("Error");
            }
        }
    }
}
