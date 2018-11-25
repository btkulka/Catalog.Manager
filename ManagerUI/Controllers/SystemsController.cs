using CatalogData;
using CatalogData.Classes.CMOs;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using ManagerUI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagerUI.Controllers
{
    public class SystemsController : CMController
    {
        // GET: Systems
        public ActionResult Index(int? systemId)
        {
            var userId = User.Identity.GetUserId();
            var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();
            CatalogSystem system = db.CatalogSystems.Where(w => (bool)w.IsActive && w.SystemId == systemId && w.BUILDERInstanceId == creds.BUILDERInstanceId).FirstOrDefault();
            List<CatalogSystem> systemHistory = db.CatalogSystems.Where(w => !(bool)w.IsActive && w.SystemId == systemId && w.BUILDERInstanceId == creds.BUILDERInstanceId).OrderByDescending(s => s.CreatedDate).ToList();

            CatalogSystemViewModel viewModel = new CatalogSystemViewModel()
            {
                System = system,
                SystemHistory = systemHistory
            };

            return View(viewModel);
        }

        public ActionResult SystemsList()
        {
            return View();
        }

        public ActionResult Systems_Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Systems_Create(FormCollection details)
        {
            try
            {
                CatalogSystem sys = new CatalogSystem()
                {
                    SystemDescription = details["SystemDescription"],
                    IsUii = details["IsUii"].Contains("true") ? true : false,
                    SystemId = Convert.ToInt32(details["SystemId"]),
                    UiiCode = details["UiiCode"]
                };

                if (sys.SystemId < 0)
                {
                    throw new Exception("The new System Id cannot be a negative number.");
                }

                if(db.CatalogSystems.Where(w => w.SystemId == sys.SystemId).Count() > 0)
                {
                    throw new Exception(String.Format("A System with SystemId {0} already exists.", sys.SystemId));
                }

                if(db.CatalogSystems.Where(w => w.UiiCode == sys.UiiCode).Count() > 0)
                {
                    throw new Exception(String.Format("A System with UII Code {0} already exists.", sys.UiiCode));
                }

                if(sys.UiiCode == "" || sys.UiiCode == null)
                {
                    throw new Exception("System UII Code cannot be blank.");
                }

                CatalogBusiness.CatalogBusiness cb = new CatalogBusiness.CatalogBusiness(FetchCredentials());
                CMOSystem cmo = new CMOSystem()
                {
                    Description = sys.SystemDescription,
                    IsUII = (bool)sys.IsUii,
                    SystemId = sys.SystemId,
                    UIICode = sys.UiiCode
                };

                var result = cb.createCatalogSystem(cmo);

                if(result.StatusCode == "200")
                {
                    var userId = User.Identity.GetUserId();
                    var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();

                    sys.Id = Guid.NewGuid();
                    sys.IsActive = true;
                    sys.CreatedByUserId = userId;
                    sys.CreatedDate = DateTime.Now;
                    sys.BUILDERInstanceId = (Guid)creds.BUILDERInstanceId;

                    db.CatalogSystems.Add(sys);
                    db.SaveChanges();
                }
                else
                {
                    throw new Exception("BUILDER was unable to create the Catalog System.");
                }

                TempData["UserAlert"] = String.Format("{0} was successfully created.", sys.SystemDescription);
                TempData["AlertType"] = "success";

                return RedirectToAction("SystemsList");
            }catch(Exception ex)
            {
                TempData["UserAlert"] = String.Format("An error occurred while attempting to create a new Catalog System: {0}", ex.Message);
                TempData["AlertType"] = "error";
                return RedirectToAction("Systems_Create");
            }
        }

        public ActionResult Systems_Read(DataSourceRequest request)
        {
            var userId = User.Identity.GetUserId();
            var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();
            var systems = db.CatalogSystems.Where(w => (bool)w.IsActive && w.BUILDERInstanceId == creds.BUILDERInstanceId).OrderBy(s => s.SystemId).ToList();

            return Json(systems.ToDataSourceResult(request, system => new CatalogSystem() {
                SystemId = system.SystemId,
                IsUii = system.IsUii,
                SystemDescription = system.SystemDescription,
                UiiCode = system.UiiCode
            }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Systems_Update(DataSourceRequest request, CatalogSystem sys)
        {
            try
            {
                if (sys.SystemId < 0)
                {
                    throw new Exception("The new System Id cannot be a negative number.");
                }

                if (sys.UiiCode == "" || sys.UiiCode == null)
                {
                    throw new Exception("System UII Code cannot be blank.");
                }

                CatalogBusiness.CatalogBusiness cb = new CatalogBusiness.CatalogBusiness(FetchCredentials());
                CMOSystem cmo = new CMOSystem()
                {
                    Description = sys.SystemDescription,
                    IsUII = (bool)sys.IsUii,
                    SystemId = sys.SystemId,
                    UIICode = sys.UiiCode
                };

                var result = cb.updateSystem(cmo);

                if (result.StatusCode == "200")
                {
                    var userId = User.Identity.GetUserId();
                    var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();
                    var oldSys = db.CatalogSystems.Where(w => w.SystemId == sys.SystemId && (bool)w.IsActive).FirstOrDefault();

                    sys.Id = Guid.NewGuid();
                    sys.IsActive = true;
                    sys.CreatedByUserId = userId;
                    sys.CreatedDate = DateTime.Now;
                    sys.BUILDERInstanceId = (Guid)creds.BUILDERInstanceId;
                    oldSys.IsActive = false;
                    db.CatalogSystems.Add(sys);
                    db.SaveChanges();
                }
                else
                {
                    throw new Exception("BUILDER was unable to update the Catalog System.");
                }

                TempData["UserAlert"] = String.Format("{0} was successfully updated.", sys.SystemDescription);
                TempData["AlertType"] = "success";

                return null;
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("An error occurred while attempting to update Catalog System: {0}", ex.Message);
                TempData["AlertType"] = "error";
                return null;
            }
        }

        [HttpPost]
        public ActionResult Systems_Edit(FormCollection details)
        {
            var systemDescription = details["System.SystemDescription"];
            var sysId = Convert.ToInt32(details["System.SystemId"]);
            var uiiCode = details["System.UiiCode"];
            var isUii = details["System.IsUii.Value"].ToLower().Contains("true");

            var sysGuid = Guid.Parse(details["System.Id"]);

            try
            {
                var orgSys = db.CatalogSystems.Where(w => w.Id == sysGuid).FirstOrDefault();

                if (orgSys.SystemDescription == systemDescription && orgSys.IsUii == isUii && orgSys.UiiCode == uiiCode)
                    throw new Exception("There were no changes to the Catalog System, so an additional record is not needed.");

                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();

                CatalogSystem newSys = new CatalogSystem()
                {
                    Id = Guid.NewGuid(),
                    UiiCode = uiiCode,
                    SystemId = sysId,
                    SystemDescription = systemDescription,
                    BUILDERInstanceId = (Guid)creds.BUILDERInstanceId,
                    CreatedByUserId = userId,
                    IsUii = isUii,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                orgSys.IsActive = false;

                db.CatalogSystems.Add(newSys);

                CatalogBusiness.CatalogBusiness cb = new CatalogBusiness.CatalogBusiness(FetchCredentials());

                var cmo = cb.updateSystem(new CMOSystem()
                {
                    Description = newSys.SystemDescription,
                    IsUII = (bool)newSys.IsUii,
                    SystemId = newSys.SystemId,
                    UIICode = newSys.UiiCode
                });

                if (cmo.StatusCode == "200")
                {
                    db.SaveChanges();
                    TempData["UserAlert"] = String.Format("{0} was successfully updated.", systemDescription);
                    TempData["AlertType"] = "success";
                }
                else
                {
                    throw new Exception(cmo.StatusDetail);
                }

                return RedirectToAction("Index", new { systemId = sysId });
            }catch(Exception ex)
            {
                TempData["UserAlert"] = String.Format("Could not update {0}: {1}", systemDescription, ex.Message);
                TempData["AlertType"] = "error";
                return RedirectToAction("Index", new { systemId = sysId });
            }
        }

        public ActionResult Systems_Delete(int? systemId)
        {
            try
            {
                CatalogBusiness.CatalogBusiness cb = new CatalogBusiness.CatalogBusiness(FetchCredentials());

                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();
                var sysInfo = db.CatalogSystems.Where(w => w.SystemId == systemId).FirstOrDefault();
                var success = cb.deleteSystem((int)systemId);

                if (success)
                {
                    foreach(CatalogSystem sys in db.CatalogSystems.Where(w => w.SystemId == systemId && w.BUILDERInstanceId == creds.BUILDERInstanceId).ToList())
                    {
                        db.CatalogSystems.Remove(sys);
                    }

                    db.SaveChanges();
                    TempData["UserAlert"] = String.Format("Successfully deleted Catalog System '{0}'.", sysInfo.SystemDescription);
                    TempData["AlertType"] = "success";
                }
                else
                {
                    throw new Exception("BUILDER failed when attempting to delete the Catalog System.");
                }
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("Was unable to delete the Catalog System: {0}.", ex.Message);
                TempData["AlertType"] = "error";
            }

            return RedirectToAction("ManageDeactivated_Systems", "Application");
        }

        public ActionResult Systems_Restore(string guid, int? sysId)
        {
            try
            {
                Guid id = Guid.Parse(guid);
                var system = db.CatalogSystems.Where(w => w.Id == id).FirstOrDefault();
                var activeSystem = db.CatalogSystems.Where(w => w.SystemId == system.SystemId && (bool)w.IsActive).FirstOrDefault();
                activeSystem.IsActive = false;
                system.IsActive = true;

                db.SaveChanges();

                TempData["UserAlert"] = String.Format("{0} restored to {1}.", system.SystemDescription, system.CreatedDate.Value.ToShortDateString());
                TempData["AlertType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("Was unable to restore to an earlier point: {0}", ex.Message);
                TempData["AlertType"] = "error";
            }

            return RedirectToAction("Index", "Systems", new { systemId = sysId });
        }

        public ActionResult Systems_Deactivate(int? sysId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => (bool)w.IsActive && w.AspNetUserId == userId).FirstOrDefault();
                var system = db.CatalogSystems.Where(w => w.SystemId == sysId && (bool)w.IsActive && creds.BUILDERInstanceId == w.BUILDERInstanceId).FirstOrDefault();
                system.IsActive = false;

                db.SaveChanges();

                TempData["UserAlert"] = String.Format("Deactivated System '{0}'. To reactivate or remove entirely, go to 'Manage Inactive Records'.", system.SystemDescription);
                TempData["AlertType"] = "success";
                return RedirectToAction("SystemsList", "Systems");
            }catch(Exception ex)
            {
                TempData["UserAlert"] = String.Format("Was unable to deactivate System: {0}", ex.Message);
                TempData["AlertType"] = "error";
                return RedirectToAction("Index", "Systems", new { systemId = sysId });
            }
        }

        [HttpPost]
        public ActionResult Excel_Export_List(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public ActionResult Systems_RestoreDeactivated(int? systemId)
        {
            try
            {
                // set the most recent iteration of the system to active
                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId & (bool)w.IsActive).FirstOrDefault();
                var system = db.CatalogSystems.Where(w => w.SystemId == systemId && w.BUILDERInstanceId == creds.BUILDERInstanceId).OrderByDescending(s => s.CreatedDate).FirstOrDefault();
                system.IsActive = true;

                db.SaveChanges();

                TempData["UserAlert"] = String.Format("Successfully restored '{0}' to {1}.", system.SystemDescription, system.CreatedDate.Value.ToShortDateString());
                TempData["AlertType"] = "success";
            }catch(Exception ex)
            {
                TempData["UserAlert"] = String.Format("Was unable to restore the Catalog System: {0}.", ex.Message);
                TempData["AlertType"] = "error";
            }

            return RedirectToAction("ManageDeactivated_Systems", "Application");
        }
    }
}