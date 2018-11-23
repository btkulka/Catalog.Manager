using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagerUI.Classes;
using Kendo.Mvc.Extensions;
using CatalogData;
using CatalogData.Classes.CMOs;
using ManagerUI.Models;

namespace ManagerUI.Controllers
{
    public class ComponentTypesController : CMController
    {
        // GET: ComponentTypes
        public ActionResult Index(int id)
        {
            // Fetch CompType and its history
            var builderId = Guid.Parse(User.Identity.GetBuilderInstanceId());
            var compType = db.ComponentTypes.Where(w => w.CompTypeId == id && w.BUILDERInstanceId == builderId && (bool)w.IsActive).FirstOrDefault();
            var compTypeHistory = db.ComponentTypes.Where(w => w.CompTypeId == id && w.BUILDERInstanceId == builderId && (bool)!w.IsActive).OrderByDescending(s => s.CreatedDate).ToList();

            // Build View Model
            ComponentTypeViewModel viewModel = new ComponentTypeViewModel();
            viewModel.CompType = compType;
            viewModel.CompTypeHistory = compTypeHistory;

            return View(viewModel);
        }

        public ActionResult ComponentTypesList()
        {
            return View();
        }

        public ActionResult ComponentTypes_Create(DataSourceRequest request)
        {
            return null;
        }

        public ActionResult ComponentTypes_Read(DataSourceRequest request)
        {
            var builderId = Guid.Parse(User.Identity.GetBuilderInstanceId());
            var componentTypes = db.ComponentTypes.Where(w => w.BUILDERInstanceId == builderId && (bool)w.IsActive).OrderBy(s => s.CompTypeId).ToList();

            return Json(componentTypes.ToDataSourceResult(request, compType => new ComponentType()
            {
                CompTypeDescription = compType.CompTypeDescription,
                CompTypeId = compType.CompTypeId
            }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ComponentTypes_Edit(FormCollection details)
        {
            var compTypeDescription = details["CompType.CompTypeDescription"];
            var compTypeId = Convert.ToInt32(details["CompType.CompTypeId"]);

            var compTypeGuid = Guid.Parse(details["CompType.Id"]);

            try
            {
                var orgCompType = db.ComponentTypes.Where(w => w.Id == compTypeGuid).FirstOrDefault();

                if (orgCompType.CompTypeDescription == compTypeDescription)
                    throw new Exception("There were no changes to the Component Type, so an additional record is not needed.");

                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();

                ComponentType newCompType = new ComponentType()
                {
                    Id = Guid.NewGuid(),
                    CompTypeDescription = compTypeDescription,
                    CompTypeId = compTypeId,
                    BUILDERInstanceId = (Guid)creds.BUILDERInstanceId,
                    CreatedByUserId = userId,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                orgCompType.IsActive = false;

                db.ComponentTypes.Add(newCompType);
                CatalogBusiness.CatalogBusiness cb = new CatalogBusiness.CatalogBusiness(FetchCredentials());

                var cmo = cb.updateComponentType(new CMOComponentType()
                {
                    Description = newCompType.CompTypeDescription,
                    ComponentTypeId = newCompType.CompTypeId
                });

                if (cmo.StatusCode == "200")
                {
                    db.SaveChanges();
                    TempData["UserAlert"] = String.Format("{0} was successfully updated.", compTypeDescription);
                    TempData["AlertType"] = "success";
                }
                else
                {
                    throw new Exception(cmo.StatusDetail);
                }

                return RedirectToAction("Index", new { id = compTypeId });
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("Could not update {0}: {1}", compTypeDescription, ex.Message);
                TempData["AlertType"] = "error";
                return RedirectToAction("Index", new { id = compTypeId });
            }
        }

        public ActionResult ComponentTypes_Update(DataSourceRequest request)
        {
            return null;
        }

        public ActionResult ComponentTypes_Delete(DataSourceRequest request)
        {
            return null;
        }

        public ActionResult ComponentTypes_Deactivate(int? compTypeId)
        {
            try
            {
                var compType = db.ComponentTypes.Where(w => w.CompTypeId == compTypeId && (bool)w.IsActive).FirstOrDefault();
                compType.IsActive = false;

                db.SaveChanges();

                TempData["UserAlert"] = String.Format("Deactivated Component Type '{0}'. To reactivate or remove entirely, go to 'Manage Inactive Records'.", compType.CompTypeDescription);
                TempData["AlertType"] = "success";
                return RedirectToAction("ComponentTypesList", "ComponentTypes");
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("Was unable to deactivate System: {0}", ex.Message);
                TempData["AlertType"] = "error";
                return RedirectToAction("Index", "Systems", new { id = compTypeId });
            }
        }

        public ActionResult ComponentTypes_Restore(string guid, int? id)
        {
            try
            {
                Guid compTypeGuid = Guid.Parse(guid);
                var compType = db.ComponentTypes.Where(w => w.Id == compTypeGuid).FirstOrDefault();
                var activeCompType = db.ComponentTypes.Where(w => w.CompTypeId == compType.CompTypeId && (bool)w.IsActive).FirstOrDefault();
                activeCompType.IsActive = false;
                compType.IsActive = true;

                CatalogBusiness.CatalogBusiness cb = new CatalogBusiness.CatalogBusiness(FetchCredentials());

                var cmo = cb.updateComponentType(new CMOComponentType()
                {
                    Description = compType.CompTypeDescription,
                    ComponentTypeId = compType.CompTypeId
                });

                if (cmo.StatusCode == "200")
                {
                    db.SaveChanges();
                    TempData["UserAlert"] = String.Format("{0} restored to {1}.", compType.CompTypeDescription, compType.CreatedDate.Value.ToShortDateString());
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

            return RedirectToAction("Index", "ComponentTypes", new { id = id });
        }

        [HttpPost]
        public ActionResult Excel_Export_List(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public ActionResult ComponentTypes_RestoreDeactivated(int? compTypeId)
        {
            try
            {
                // set the most recent iteration of the system to active
                var compType = db.ComponentTypes.Where(w => w.CompTypeId == compTypeId).OrderByDescending(s => s.CreatedDate).FirstOrDefault();
                compType.IsActive = true;

                db.SaveChanges();

                TempData["UserAlert"] = String.Format("Successfully restored '{0}' to {1}.", compType.CompTypeDescription, compType.CreatedDate.Value.ToShortDateString());
                TempData["AlertType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("Was unable to restore the Component Type: {0}.", ex.Message);
                TempData["AlertType"] = "error";
            }

            return RedirectToAction("ManageDeactivated_ComponentTypes", "Application");
        }
    }
}