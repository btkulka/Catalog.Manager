using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using CatalogData;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagerUI.Models;
using CatalogData.Classes.CMOs;

namespace ManagerUI.Controllers
{
    public class ComponentsController : CMController
    {
        public ActionResult Index(int? compId)
        {
            var userId = User.Identity.GetUserId();
            var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();
            Component comp = db.Components.Where(w => (bool)w.IsActive && w.ComponentId == compId && w.BUILDERInstanceId == creds.BUILDERInstanceId).FirstOrDefault();
            List<Component> componentHistory = db.Components.Where(w => !(bool)w.IsActive && w.ComponentId == compId && w.BUILDERInstanceId == creds.BUILDERInstanceId).OrderByDescending(s => s.CreatedDate).ToList();

            ComponentViewModel viewModel = new ComponentViewModel()
            {
                Component = comp,
                History = componentHistory
            };

            return View(viewModel);
        }

        public ActionResult ComponentsList()
        {
            return View();
        }

        public ActionResult Components_Create(DataSourceRequest request)
        {
            return null;
        }

        public ActionResult Components_Read(DataSourceRequest request)
        {
            var userId = User.Identity.GetUserId();
            var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();
            var components = db.Components.Where(w => w.BUILDERInstanceId == creds.BUILDERInstanceId && (bool)w.IsActive).ToList();

            DataSourceResult result = components.ToDataSourceResult(request, component => new Component()
            {
               Description = component.Description,
               IsEquip = component.IsEquip,
               IsUii = component.IsUii,
               UiiCode = component.UiiCode,
               SystemId = component.SystemId,
               ComponentId = component.ComponentId
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Components_Update(DataSourceRequest request)
        {
            return null;
        }

        [HttpGet]
        public ActionResult Components_Deactivate(int? compId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => (bool)w.IsActive && w.AspNetUserId == userId).FirstOrDefault();
                var comp = db.Components.Where(w => w.ComponentId == compId && (bool)w.IsActive && w.BUILDERInstanceId == creds.BUILDERInstanceId).FirstOrDefault();
                comp.IsActive = false;

                db.SaveChanges();

                TempData["UserAlert"] = String.Format("Deactivated Component '{0}'. To reactivate or remove entirely, go to 'Manage Inactive Records'.", comp.Description);
                TempData["AlertType"] = "success";
                return RedirectToAction("ComponentsList", "Components");
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("Was unable to deactivate Component: {0}", ex.Message);
                TempData["AlertType"] = "error";
                return RedirectToAction("Index", "Components", new { compId = compId });
            }
        }

        public ActionResult Components_Edit(FormCollection details)
        {
            var description = details["Component.Description"];
            var compId = Convert.ToInt32(details["Component.ComponentId"]);
            var sysId = Convert.ToInt32(details["Component.SystemId"]);
            var uiiCode = details["Component.UiiCode"];
            var isUii = details["Component.IsUii.Value"].ToLower().Contains("true");
            var isEquip = details["Component.IsEquip.Value"].ToLower().Contains("true");
            var compGuid = Guid.Parse(details["Component.Id"]);

            try
            {
                var orgComp = db.Components.Where(w => w.Id == compGuid).FirstOrDefault();

                if (orgComp.Description == description && orgComp.IsUii == isUii && orgComp.UiiCode == uiiCode && orgComp.SystemId == sysId && orgComp.IsEquip == isEquip)
                    throw new Exception("There were no changes to the Component, so an additional record is not needed.");

                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();

                Component newComp = new Component()
                {
                    Id = Guid.NewGuid(),
                    UiiCode = uiiCode,
                    SystemId = sysId,
                    ComponentId = compId,
                    Description = description,
                    BUILDERInstanceId = (Guid)creds.BUILDERInstanceId,
                    CreatedByUserId = userId,
                    IsUii = isUii,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    IsEquip = isEquip
                };

                orgComp.IsActive = false;

                db.Components.Add(newComp);

                CatalogBusiness.CatalogBusiness cb = new CatalogBusiness.CatalogBusiness(FetchCredentials());

                var cmo = cb.updateComponent(new CMOComponent()
                {
                    Description = newComp.Description,
                    IsUII = (bool)newComp.IsUii,
                    SystemId = newComp.SystemId,
                    UIICode = newComp.UiiCode,
                    IsEquip = (bool)newComp.IsEquip,
                    ComponentId = newComp.ComponentId
                });

                if (cmo.StatusCode == "200")
                {
                    db.SaveChanges();
                    TempData["UserAlert"] = String.Format("{0} was successfully updated.", description);
                    TempData["AlertType"] = "success";
                }
                else
                {
                    throw new Exception(cmo.StatusDetail);
                }

                return RedirectToAction("Index", new { compId = compId });
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("Could not update {0}: {1}", description, ex.Message);
                TempData["AlertType"] = "error";
                return RedirectToAction("Index", new { compId = compId });
            }
        }

        public ActionResult Components_RestoreDeactivated(int? compId)
        {
            try
            {
                // set the most recent iteration of the system to active
                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId & (bool)w.IsActive).FirstOrDefault();
                var comp = db.Components.Where(w => w.ComponentId == compId && w.BUILDERInstanceId == creds.BUILDERInstanceId).OrderByDescending(s => s.CreatedDate).FirstOrDefault();
                comp.IsActive = true;

                db.SaveChanges();

                TempData["UserAlert"] = String.Format("Successfully restored '{0}' to {1}.", comp.Description, comp.CreatedDate.Value.ToShortDateString());
                TempData["AlertType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("Was unable to restore the Component: {0}.", ex.Message);
                TempData["AlertType"] = "error";
            }

            return RedirectToAction("ManageDeactivated_Components", "Application");
        }

        public ActionResult Components_Delete(DataSourceRequest request)
        {
            return null;
        }

        [HttpPost]
        public ActionResult Excel_Export_List(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}