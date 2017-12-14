using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Intranet.Models;
using Intranet.Utils;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Intranet.ViewModels;

namespace Intranet.Controllers
{
    public class InternationalController : Controller
    {
        private transportEntities _db = new transportEntities();

        //
        // GET: /Freight/

        public ActionResult Index()
        {
            if (User.IsInRole("LAN\\TR_Managers") || AccountManager.IsApprover(User.Identity.Name).Item2 ||
                User.IsInRole("LAN\\TR_Viewers"))
            {
                var model = _db.RequestInternationals
                    .Where(rf => (rf.Request.IsDeleted == false || rf.Request.IsDeleted == null))
                    .OrderByDescending(rf => rf.Request.PublishDate)
                    .ToList();

                return View(model);
            }
            else
            {
                var model = _db.RequestInternationals
                    .Where(
                        rf =>
                            ((rf.Request.IsDeleted == false || rf.Request.IsDeleted == null) &&
                             rf.Request.UserLogin == User.Identity.Name))
                    .OrderByDescending(rf => rf.Request.PublishDate)
                    .ToList();

                return View(model);
            }
        }

        //
        // GET: /Freight/Details/5

        public ActionResult Details(int id)
        {
            var model = _db.RequestInternationals.Single(rf => rf.RequestId == id);

            //if ((model.Request.UserLogin != User.Identity.Name) && !User.IsInRole("LAN\\TR_Managers"))
            //{
            //    ViewBag.BackController = "Freight";
            //    return View("Denied");
            //}

            //Показать детали  только автору, менеджеру, аппруверу, админу, либо вьюверу
            if (model.Request.UserLogin == User.Identity.Name || User.IsInRole("LAN\\TR_Managers") ||
                AccountManager.IsApprover(User.Identity.Name).Item2 || User.IsInRole("LAN\\TR_Admins") ||
                User.IsInRole("LAN\\TR_Viewers"))
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

        public ActionResult Create(int? id)
        {
            var model = new RequestInternational();

            if (id != null)
            {
                model = _db.Requests.Single(rf => rf.RequestId == id).RequestInternational;
                model.RequestId = 0;

                model.Request.RequestEvents = null;
                model.Request.RequestAttachments = null;
            }

            try
            {
                if (model.Request == null)
                {
                    model.Request = new Request();
                }

                var login = User.Identity.Name;
                var fio = AccountManager.GetUserDisplayName(login);
                var phone = AccountManager.GetUserPhoneNumber(login);

                model.Request.Responsible = String.Format("{0}, тел.{1}", fio, phone);
            }
            catch (Exception ex)
            {
                model.Request.Responsible = ex.Message;
            }
            return View(model);
        }


        //
        // POST: /Freight/Create

        [HttpPost]
        public ActionResult Create(RequestInternational rf, IEnumerable<HttpPostedFileBase> files)
        {
            if (rf.Request.ApproverEmployeeId == 0)
            {
                ViewBag.ErrorMessage = "Веберите руководителя, который подпишет заявку!";
                return View(rf);
            }

            if ((rf.Request.DepartmentGroupId ?? 0) == 0)
            {
                ViewBag.ErrorMessage = "Веберите структурное подразделение!";
                return View(rf);
            }

            if ((rf.Request.DirectionId ?? 0) == 0)
            {
                ViewBag.ErrorMessage = "Веберите направление перевозки!";
                return View(rf);
            }


            var Way = "Внутри Республики Беларусь";


            if (rf.Way == Way)
            {
                rf.DepartureCustoms = "";
                rf.ReturnCustoms = "";
                rf.Code = "";
            }
            else
            {
                rf.Request.AgreementPurposeId = null;
            }


            if (rf.Way == Way && (rf.Request.AgreementPurposeId ?? 0) == 0)
            {
                ViewBag.ErrorMessage = "Веберите цель перевозки!";
                return View(rf);
            }


            if (rf.Way != Way && String.IsNullOrEmpty(rf.DepartureCustoms))
            {
                ViewBag.ErrorMessage = "Таможня отправления обязательно для заполнения";
                return View(rf);
            }
            if (rf.Way != Way && String.IsNullOrEmpty(rf.ReturnCustoms))
            {
                ViewBag.ErrorMessage = "Таможня назначения обязательно для заполнения";
                return View(rf);
            }
            if (rf.Way != Way && String.IsNullOrEmpty(rf.PackageListNumber))
            {
                ViewBag.ErrorMessage = "№ упаковочного листа обязательно для заполнения";
                return View(rf);
            }
            if (rf.Way != Way && String.IsNullOrEmpty(rf.Code))
            {
                ViewBag.ErrorMessage = "Код ТНВЭД обязательно для заполнения";
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

                if (rf.Request.RequestTypeId == 0)
                    rf.Request.RequestTypeId = null;

                var isApproved = false;

                if (Utils.AccountManager.IsApprover(User.Identity.Name).Item2)
                {
                    isApproved = true;
                    rf.Request.Status = 1;
                    rf.Request.ApproveDate = DateTime.Now;
                    rf.Request.ApproverLogin = User.Identity.Name;
                    rf.Request.ApproverFio = AccountManager.GetUserDisplayName(User.Identity.Name);

                    rf.Request.RequestEvents.Add(new RequestEvent {Status = 1, EventDate = DateTime.Now});
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

                // throw new Exception("Debug");


                _db.RequestInternationals.Add(rf);
                _db.SaveChanges();

                if (isApproved)
                {
                    //var tel = AccountManager.GetUserPhoneNumber(rf.Request.UserLogin);
                    new EmailSender().Send(rf.RequestId, rf.Request.Responsible, "", rf.Way);
                }

                return View("Published", rf.Request.RequestId);
            }
            catch (DbEntityValidationException dbEx)
            {

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                            validationError.ErrorMessage);
                        //TODO: Попытка удалить запись, у которой ExpDate истек
                    }
                }

                throw (dbEx);

                ViewBag.ErrMessage = "Ошибка при создании записи:";
                ViewBag.BackController = "Home";
                return View("Error");
            }

        }


        //
        // GET: /Freight/Edit/5

        public ActionResult Edit(int id)
        {
            var model = _db.RequestInternationals.Single(rf => rf.RequestId == id);

            //if ((model.Request.UserLogin != User.Identity.Name) && !User.IsInRole("LAN\\TR_Managers"))
            //{
            //    ViewBag.BackController = "Freight";
            //    return View("Denied");
            //}

            if (model.Request.SpecTransReceived != null && model.Request.SpecTransReceived.Value)
            {
                ViewBag.BackController = "Home";
                return View("Sended");
            }

            //редактировать только автору, менеджеру, админу
            if (model.Request.UserLogin == User.Identity.Name || User.IsInRole("LAN\\TR_Managers") ||
                User.IsInRole("LAN\\TR_Admins"))
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
        public ActionResult Edit(RequestInternational rf, IEnumerable<HttpPostedFileBase> files, int[] oldFiles)
        {
            try
            {
                var model = _db.RequestInternationals.Single(m => m.RequestId == rf.RequestId);

                model.Brutto = rf.Brutto;
                model.CargoVolume = rf.CargoVolume;
                model.Netto = rf.Netto;
                model.Way = rf.Way;
                model.CargoCost = rf.CargoCost;
                model.CargoDimensions = rf.CargoDimensions;
                model.CargoName = rf.CargoName;
                model.CargoOverloading = rf.CargoOverloading;
                model.CargoStackability = rf.CargoStackability;
                model.Consignor = rf.Consignor;
                model.Code = rf.Code;
                model.UnloadingContactName = rf.UnloadingContactName;
                model.DangerCargo = rf.DangerCargo;
                model.DeliveryAddress = rf.DeliveryAddress;
                model.DeliveryBasis = rf.DeliveryBasis;
                model.DeliveryDate = rf.DeliveryDate;
                model.DepartureCustoms = rf.DepartureCustoms;
                model.LoadingContactName = rf.LoadingContactName;
                model.LoadingType = rf.LoadingType;
                model.PackageListNumber = rf.PackageListNumber;
                model.ReturnCustoms = rf.ReturnCustoms;
                model.Request.OtherInformation = rf.Request.OtherInformation;
                model.VehicleCount = rf.VehicleCount;
                model.Request.RequestDate = rf.Request.RequestDate;
                model.Request.ApproverEmployeeId = rf.Request.ApproverEmployeeId;
                model.VehicleCapacityTonns = rf.VehicleCapacityTonns;
                model.VehicleType = rf.VehicleType;
                model.VehicleCount = rf.VehicleCount;
                model.Request.Responsible = rf.Request.Responsible;
                model.Currency = rf.Currency;
                model.DeliveryDateEnd = rf.DeliveryDateEnd;
                model.RequestDateEnd = rf.RequestDateEnd;
                model.CargoPlaces = rf.CargoPlaces;




                if (rf.Request.CustomerId == 0)
                    rf.Request.CustomerId = null;
                model.Request.CustomerId = rf.Request.CustomerId;

                if (rf.Request.RequestTypeId == 0)
                    rf.Request.RequestTypeId = null;
                model.Request.RequestTypeId = rf.Request.RequestTypeId;

                if (model.Request.Status == 3 && User.Identity.Name.ToLower() == model.Request.UserLogin.ToLower())
                {
                    model.Request.Status = 0;
                    var re = new RequestEvent {Status = 0, Message = String.Empty, EventDate = DateTime.Now};
                    model.Request.RequestEvents.Add(re);
                }

                //Удаляем файлы, которые пользователь отметил как удаленные                
                var attForRemoving = new List<RequestAttachment>();
                if (oldFiles != null) //Если остался хоть один старый файл
                {
                    attForRemoving = model.Request.RequestAttachments.Where(a => !oldFiles.Contains(a.Id)).ToList();
                }
                else //Если пользователь удаляет все файлы
                {
                    attForRemoving =
                        model.Request.RequestAttachments.Where(a => (a.IsDeleted == false || a.IsDeleted == null))
                            .ToList();
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
                            var attFile = new RequestAttachment
                            {
                                Name = file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1)
                            };

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
            var model = _db.RequestInternationals.Single(rf => rf.RequestId == id);

            //if ((model.Request.UserLogin != User.Identity.Name) && !User.IsInRole("LAN\\TR_Managers"))
            //{
            //    ViewBag.BackController = "Freight";
            //    return View("Denied");
            //}
            //return View(model);

            if (model.Request.SpecTransReceived != null && model.Request.SpecTransReceived.Value)
            {
                ViewBag.BackController = "Home";
                return View("Sended");
            }
            

            //удалять только автору, менеджеру, админу
            if (model.Request.UserLogin == User.Identity.Name || User.IsInRole("LAN\\TR_Managers") ||
                User.IsInRole("LAN\\TR_Admins"))
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
                var model = _db.RequestInternationals.Single(rf => rf.RequestId == id);
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
                var model = _db.RequestInternationals.Single(rf => rf.RequestId == id);
                return View(model);
            }
            else
            {
                ViewBag.ErrMessage = "У текущего пользователя отсутствует право подаписи заявок!";
                ViewBag.BackController = "Home";
                return View("Error");
            }
        }


        public ActionResult SendToSpectrans(int id)
        {
            if (User.IsInRole("LAN\\TR_Managers"))
            {
                var model = _db.RequestInternationals.Single(rf => rf.RequestId == id);
                return View(model);
            }
            else
            {
                ViewBag.ErrMessage = "У текущего пользователя отсутствует право отправки заявок в Спецтранс!";
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


        public PartialViewResult FilterRequests(string userFio)
        {
            var indexList = new InternationalIndexList();
            DateTime requestDate;

            var model =
                _db.RequestInternationals.Where(r => (r.Request.IsDeleted != true || r.Request.IsDeleted == null));
            if (User.IsInRole("LAN\\TR_Managers") || AccountManager.IsApprover(User.Identity.Name).Item2 ||
                User.IsInRole("LAN\\TR_Viewers"))
            {
            }
            else
            {
                model = model.Where(r => r.Request.UserLogin == User.Identity.Name);
            }

            if (!String.IsNullOrEmpty(userFio))
            {
                model = model.Where(x => x.Request.UserFio.Contains(userFio));
            }


            var rezult = model.OrderByDescending(r => r.Request.PublishDate)
                .ToList();


            foreach (var r in rezult)
            {
                r.Request.DetailsLink = RequestType.GetDetailsLink(r.Request);
                r.Request.DetailsButton = RequestType.GetDetailsButton(r.Request);
                r.Request.CopyButton = RequestType.GetCopyButton(r.Request);
                r.Request.RequestTypeLabel = RequestType.GetRequestTypeLabel(r.Request);
            }

            indexList.Requests = rezult;
            indexList.TotalRows = rezult.Count;
            indexList.RowPerPage = rezult.Count;
            return PartialView("_Table", indexList.Requests);

        }
    }
}
