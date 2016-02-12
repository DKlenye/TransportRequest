using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Intranet.Models;
using System.Collections;
using Intranet.Utils;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Intranet.Controllers
{
    public class FreightController : Controller
    {
        transportEntities _db = new transportEntities();

        //
        // GET: /Freight/

        public ActionResult Index()
        {
            if (User.IsInRole("LAN\\TR_Managers") || AccountManager.IsApprover(User.Identity.Name).Item2 || User.IsInRole("LAN\\TR_Viewers"))
            {
                var model = _db.RequestFreights
                   .Where(rf => (rf.Request.IsDeleted == false || rf.Request.IsDeleted == null))
                   .OrderByDescending(rf => rf.Request.PublishDate)
                   .ToList();

                return View(model);
            }
            else
            {
                var model = _db.RequestFreights
                   .Where(rf => ((rf.Request.IsDeleted == false || rf.Request.IsDeleted == null) && rf.Request.UserLogin == User.Identity.Name))
                   .OrderByDescending(rf => rf.Request.PublishDate)
                   .ToList();

                return View(model);
            }            
        }

        //
        // GET: /Freight/Details/5

        public ActionResult Details(int id)
        {
            var model = _db.RequestFreights.Single(rf => rf.RequestId == id);
            model.RequestFreightCargoes = model.RequestFreightCargoes.Where(rfc => rfc.IsDeleted == false).ToList();

            //if ((model.Request.UserLogin != User.Identity.Name) && !User.IsInRole("LAN\\TR_Managers"))
            //{
            //    ViewBag.BackController = "Freight";
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
        // GET: /Freight/Create

        public ActionResult Create()
        {
            var model = new RequestFreight();
            model.RequestFreightCargoes.Add(new RequestFreightCargo());
            return View(model);
        } 

        //
        // POST: /Freight/Create

        [HttpPost]
        public ActionResult Create(RequestFreight rf, IEnumerable<HttpPostedFileBase> files)
        {
            if (rf.Request.ApproverEmployeeId == 0)
            {
                    ViewBag.ErrorMessage = "Веберите руководителя, который подпишет заявку!";
                    return View(rf);             
            }

            try
            {
                rf.Request.Status = 0;
                rf.Request.UserLogin = User.Identity.Name;
                rf.Request.UserFio = AccountManager.GetUserDisplayName(User.Identity.Name);
                rf.Request.PublishDate = DateTime.Now;
                rf.Request.IsDeleted = false;

                if (rf.Request.CustomerId == 0)
                    rf.Request.CustomerId = null;

                if (Utils.AccountManager.IsApprover(User.Identity.Name).Item2)
                {
                    rf.Request.Status = 1;
                    rf.Request.ApproveDate = DateTime.Now;
                    rf.Request.ApproverLogin = User.Identity.Name;
                    rf.Request.ApproverFio = Utils.AccountManager.GetUserDisplayName(User.Identity.Name);

                    RequestEvent re = new RequestEvent();
                    re.Status = 1;
                    re.EventDate = DateTime.Now;
                    rf.Request.RequestEvents.Add(re);
                }

                //Файлы
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file != null)
                        {
                            RequestAttachment attFile = new RequestAttachment();
                            attFile.Name = file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1);

                            int fl = file.ContentLength;
                            byte[] fc = new byte[fl];

                            file.InputStream.Read(fc, 0, fl);

                            attFile.Cont = fc;

                            rf.Request.RequestAttachments.Add(attFile);
                        }
                    }
                }

                _db.RequestFreights.Add(rf);
                _db.SaveChanges();

                return View("Published", rf.Request.RequestId);
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

        public ActionResult AddCargoRow(int id)
        {
            return PartialView("_CargoAddRow", id);
        }
        
        //
        // GET: /Freight/Edit/5
 
        public ActionResult Edit(int id)
        {
            var model = _db.RequestFreights.Single(rf => rf.RequestId == id);
            model.RequestFreightCargoes = model.RequestFreightCargoes.Where(rfc => rfc.IsDeleted == false).ToList();

            //if ((model.Request.UserLogin != User.Identity.Name) && !User.IsInRole("LAN\\TR_Managers"))
            //{
            //    ViewBag.BackController = "Freight";
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
        // POST: /Freight/Edit/5

        [HttpPost]
        public ActionResult Edit(RequestFreight rf, IEnumerable<HttpPostedFileBase> files, int[] oldFiles)
        {
            try
            {
                var model = _db.RequestFreights.Single(m => m.RequestId == rf.RequestId);
                var respCargoes = rf.RequestFreightCargoes;

                model.DestinationPoint = rf.DestinationPoint;
                model.OrderNumber = rf.OrderNumber;
                model.LoadingType = rf.LoadingType;
                model.LoadingAddress = rf.LoadingAddress;
                model.ContactName = rf.ContactName;
                model.Responsible = rf.Responsible;
                model.Request.RequestDate = rf.Request.RequestDate;
                model.Request.ApproverEmployeeId = rf.Request.ApproverEmployeeId;
                model.VehicleType = rf.VehicleType;

                if (rf.Request.CustomerId == 0)
                    rf.Request.CustomerId = null;
                model.Request.CustomerId = rf.Request.CustomerId;

                if (model.Request.Status == 3 && User.Identity.Name.ToLower() == model.Request.UserLogin.ToLower())
                {
                    model.Request.Status = 0;
                    RequestEvent re = new RequestEvent();
                    re.Status = 0;
                    re.Message = String.Empty;
                    re.EventDate = DateTime.Now;
                    model.Request.RequestEvents.Add(re);
                }

                var modelCargoes = model.RequestFreightCargoes;

                foreach (var modelCargo in modelCargoes)
                {
                    //Существует ли такой груз в респонсе
                    bool cargoExistInResponse = false;

                    foreach(var respCargo in respCargoes)
                    {
                        //Если обновление груза
                        if (respCargo.CargoId == modelCargo.CargoId)
                        {
                            modelCargo.CargoName = respCargo.CargoName;
                            modelCargo.Weight = respCargo.Weight;
                            modelCargo.Length = respCargo.Length;
                            modelCargo.Height = respCargo.Height;
                            modelCargo.Width = respCargo.Width;
                            modelCargo.Volume = respCargo.Volume;

                            cargoExistInResponse = true;
                        }
                    }

                    if (cargoExistInResponse == false)
                        modelCargo.IsDeleted = true;
                }

                //Добавляем все новые грузы, т.е те, у которых CargoId == 0
                foreach (var respCargo in respCargoes)
                    if (respCargo.CargoId == 0)
                        modelCargoes.Add(respCargo);

                //Удаляем файлы, которые пользователь отметил как удаленные                
                var attForRemoving = new List<RequestAttachment>();
                if (oldFiles != null) //Если остался хоть один старый файл
                {
                    attForRemoving = model.Request.RequestAttachments.Where(a => !oldFiles.Contains(a.Id)).ToList();
                }
                else //Если пользователь удаляет все файлы
                {
                    attForRemoving = model.Request.RequestAttachments.Where(a => (a.IsDeleted == false || a.IsDeleted == null)).ToList();
                }
                foreach (var att in attForRemoving)
                {
                    att.IsDeleted = true;
                }

                //Добавляем новые файлы
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file != null)
                        {
                            RequestAttachment attFile = new RequestAttachment();
                            attFile.Name = file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1);

                            int fl = file.ContentLength;
                            byte[] fc = new byte[fl];

                            file.InputStream.Read(fc, 0, fl);

                            attFile.Cont = fc;
                            attFile.IsDeleted = false;

                            model.Request.RequestAttachments.Add(attFile);
                        }
                    }
                }
                
                _db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ViewBag.ErrMessage = "Ошибка при редактировании записи";
                ViewBag.BackController = "Freight";
                return View("Error");
            }
        }

        //
        // GET: /Freight/Delete/5
 
        public ActionResult Delete(int id)
        {
            var model = _db.RequestFreights.Single(rf => rf.RequestId == id);

            //if ((model.Request.UserLogin != User.Identity.Name) && !User.IsInRole("LAN\\TR_Managers"))
            //{
            //    ViewBag.BackController = "Freight";
            //    return View("Denied");
            //}
            //return View(model);

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
        // POST: /Freight/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var model = _db.RequestFreights.Single(rf => rf.RequestId == id);
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
                var model = _db.RequestFreights.Single(rf => rf.RequestId == id);
                return View(model);
            }
            else
            {
                ViewBag.ErrMessage = "У текущего пользователя отсутствует право подаписи заявок!";
                ViewBag.BackController = "Home";
                return View("Error");
            }
        }
  

        public ActionResult FileAddRow()
        {
            return PartialView("_FileAddRow", new RequestAttachment());
        }

        public ActionResult DownloadFile(int fId)
        {
            try
            {
                var file = _db.RequestAttachments.Single(f => f.Id == fId);
                if (file.IsDeleted == true)
                    throw new HttpException(404, "Не могу найти файл " + fId.ToString() + ". Возможно он был удален :(");
                else
                    return File(file.Cont, "application/zip", Utils.Translit.TranslitName(file.Name));
            }
            catch
            {
                throw new HttpException(404, "Couldn't find file" + fId.ToString());
            }

        }
    }
}
