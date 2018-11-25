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
    public class SubComponentsController : CMController
    {
        public ActionResult Index(int? subComponentId)
        {
            // fetch subcomponent details
            var userId = User.Identity.GetUserId();
            var creds = db.CatalogCredentials.Where(w => (bool)w.IsActive && w.AspNetUserId == userId).FirstOrDefault();
            var subComponent = db.SubComponents.Where(w => (bool)w.IsActive && w.BUILDERInstanceId == creds.BUILDERInstanceId && w.SubComponentId == subComponentId).FirstOrDefault();
            var subComponentHistory = db.SubComponents.Where(w => (bool)!w.IsActive && w.BUILDERInstanceId == creds.BUILDERInstanceId && w.SubComponentId == subComponentId).OrderByDescending(s => s.CreatedDate).ToList();

            // create view model
            SubComponentViewModel viewModel = new SubComponentViewModel();
            viewModel.SubComp = subComponent;
            viewModel.SubCompHistory = subComponentHistory;

            return View(viewModel);
        }

        public ActionResult SubComponentsList()
        {
            return View();
        }

        public ActionResult SubComponents_Create(DataSourceRequest request)
        {
            return null;
        }

        public ActionResult SubComponents_Read(DataSourceRequest request)
        {
            var userId = User.Identity.GetUserId();
            var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();
            var subComponents = db.SubComponents.Where(w => w.BUILDERInstanceId == creds.BUILDERInstanceId && (bool)w.IsActive).OrderBy(s => s.SubComponentId).ToList();

            return Json(subComponents.ToDataSourceResult(request, subComponent => new SubComponent()
            {
                SubComponentDescription = subComponent.SubComponentDescription,
                SubComponentId = subComponent.SubComponentId,
                SubCompUnitId = subComponent.SubCompUnitId
            }));
        }

        public ActionResult SubComponents_Update(DataSourceRequest request)
        {
            return null;
        }

        public ActionResult SubComponents_Delete(DataSourceRequest request)
        {
            return null;
        }

        public ActionResult SubComponents_Edit(FormCollection details)
        {
            var subCompDescription = details["SubComp.SubComponentDescription"];
            var subCompId = Convert.ToInt32(details["SubComp.SubComponentId"]);
            var subCompUnitId = Convert.ToInt32(details["SubComp.SubCompUnitId"]);

            var subCompGuid = Guid.Parse(details["SubComp.Id"]);

            try
            {
                var orgSubComp = db.SubComponents.Where(w => w.Id == subCompGuid).FirstOrDefault();

                if (orgSubComp.SubComponentDescription == subCompDescription)
                    throw new Exception("There were no changes to the SubComponent, so an additional record is not needed.");

                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();

                SubComponent newSubComp = new SubComponent()
                {
                    Id = Guid.NewGuid(),
                    SubComponentDescription = subCompDescription,
                    SubComponentId = subCompId,
                    SubCompUnitId = subCompUnitId,
                    BUILDERInstanceId = (Guid)creds.BUILDERInstanceId,
                    CreatedByUserId = userId,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                orgSubComp.IsActive = false;

                db.SubComponents.Add(newSubComp);
                CatalogBusiness.CatalogBusiness cb = new CatalogBusiness.CatalogBusiness(FetchCredentials());

                var cmo = cb.updateSubComponent(new CMOSubComponent()
                {
                    Description = newSubComp.SubComponentDescription,
                    SubComponentId = newSubComp.SubComponentId,
                    CompUnitsId = newSubComp.SubCompUnitId
                });

                if (cmo.StatusCode == "200")
                {
                    db.SaveChanges();
                    TempData["UserAlert"] = String.Format("{0} was successfully updated.", subCompDescription);
                    TempData["AlertType"] = "success";
                }
                else
                {
                    throw new Exception(cmo.StatusDetail);
                }

                return RedirectToAction("Index", new { subComponentId = subCompId });
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("Could not update {0}: {1}", subCompDescription, ex.Message);
                TempData["AlertType"] = "error";
                return RedirectToAction("Index", new { subComponentId = subCompId });
            }
        }

        public ActionResult SubComponents_Restore(string guid, int? id)
        {
            try
            {
                Guid subCompGuid = Guid.Parse(guid);
                var subComp = db.SubComponents.Where(w => w.Id == subCompGuid).FirstOrDefault();
                var activeSubComp = db.SubComponents.Where(w => w.SubComponentId == subComp.SubComponentId && (bool)w.IsActive).FirstOrDefault();
                activeSubComp.IsActive = false;
                subComp.IsActive = true;

                CatalogBusiness.CatalogBusiness cb = new CatalogBusiness.CatalogBusiness(FetchCredentials());

                var cmo = cb.updateSubComponent(new CMOSubComponent()
                {
                    Description = subComp.SubComponentDescription,
                    SubComponentId = subComp.SubComponentId,
                    CompUnitsId = subComp.SubCompUnitId
                });

                if (cmo.StatusCode == "200")
                {
                    db.SaveChanges();
                    TempData["UserAlert"] = String.Format("{0} restored to {1}.", subComp.SubComponentDescription, subComp.CreatedDate.Value.ToShortDateString());
                    TempData["AlertType"] = "success";
                }
                else
                {
                    throw new Exception(cmo.StatusDetail);
                }
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("Was unable to restore to an earlier point: {0}", ex.Message);
                TempData["AlertType"] = "success";
            }

            return RedirectToAction("Index", new { subComponentId = id });
        }

        public ActionResult SubComponents_Deactivate(int? subCompId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => (bool)w.IsActive && w.AspNetUserId == userId).FirstOrDefault();
                var subComp = db.SubComponents.Where(w => w.SubComponentId == subCompId && (bool)w.IsActive && w.BUILDERInstanceId == creds.BUILDERInstanceId).FirstOrDefault();
                subComp.IsActive = false;

                db.SaveChanges();

                TempData["UserAlert"] = String.Format("Deactivated SubComponent '{0}'. To reactivate or remove entirely, go to 'Manage Inactive Records'.", subComp.SubComponentDescription);
                TempData["AlertType"] = "success";
                return RedirectToAction("SubComponentsList", "SubComponents");
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("Was unable to deactivate SubComponent: {0}", ex.Message);
                TempData["AlertType"] = "error";
                return RedirectToAction("Index", new { subComponentId = subCompId });
            }
        }

        [HttpPost]
        public ActionResult Excel_Export_List(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public ActionResult SubComponents_RestoreDeactivated(int? subCompId)
        {
            try
            {
                // set the most recent iteration of the system to active// set the most recent iteration of the system to active
                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId & (bool)w.IsActive).FirstOrDefault();
                var subComp = db.SubComponents.Where(w => w.SubComponentId == subCompId && w.BUILDERInstanceId == creds.BUILDERInstanceId).OrderByDescending(s => s.CreatedDate).FirstOrDefault();
                subComp.IsActive = true;

                db.SaveChanges();

                TempData["UserAlert"] = String.Format("Successfully restored '{0}' to {1}.", subComp.SubComponentDescription, subComp.CreatedDate.Value.ToShortDateString());
                TempData["AlertType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("Was unable to restore the SubComponent: {0}.", ex.Message);
                TempData["AlertType"] = "error";
            }

            return RedirectToAction("ManageDeactivated_SubComponents", "Application");
        }
    }
}