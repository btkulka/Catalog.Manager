using CatalogData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagerUI.Models;
using CatalogData.Classes.CMOs;

namespace ManagerUI.Controllers
{
    public class MaterialCategoriesController : CMController
    {
        public ActionResult Index(int? matCatId)
        {
            // fetch members
            var userId = User.Identity.GetUserId();
            var creds = db.CatalogCredentials.Where(w => (bool)w.IsActive && w.AspNetUserId == userId).FirstOrDefault();
            var matCat = db.MaterialCategories.Where(w => w.MatCatId == matCatId && (bool)w.IsActive && w.BUILDERInstanceId == creds.BUILDERInstanceId).FirstOrDefault();
            var matCatHistory = db.MaterialCategories.Where(w => w.MatCatId == matCatId && (bool)!w.IsActive && w.BUILDERInstanceId == creds.BUILDERInstanceId).OrderByDescending(s => s.CreatedDate).ToList();

            // create view model
            MaterialCategoriesViewModel viewModel = new MaterialCategoriesViewModel();
            viewModel.MatCat = matCat;
            viewModel.MatCatHistory = matCatHistory;

            return View(viewModel);
        }

        public ActionResult MaterialCategoriesList()
        {
            return View();
        }

        public ActionResult MaterialCategories_Create(DataSourceRequest request)
        {
            return null;
        }

        public ActionResult MaterialCategories_Read(DataSourceRequest request)
        {
            var userId = User.Identity.GetUserId();
            var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();
            var matCats = db.MaterialCategories.Where(w => w.BUILDERInstanceId == creds.BUILDERInstanceId && (bool)w.IsActive).OrderBy(s => s.MatCatId).ToList();

            return Json(matCats.ToDataSourceResult(request, matCat => new MaterialCategory()
            {
                MatCatId = matCat.MatCatId,
                MatCatDescription = matCat.MatCatDescription
            }), JsonRequestBehavior.AllowGet);
        }
        public ActionResult MaterialCategories_Update(DataSourceRequest request)
        {
            return null;
        }
        public ActionResult MaterialCategories_Delete(DataSourceRequest request)
        {
            return null;
        }

        public ActionResult MaterialCategories_Edit(FormCollection details)
        {
            var matCatDescription = details["MatCat.MatCatDescription"];
            var matCatId = Convert.ToInt32(details["MatCat.MatCatId"]);

            var matCatGuid = Guid.Parse(details["MatCat.Id"]);

            try
            {
                var orgMatCat = db.MaterialCategories.Where(w => w.Id == matCatGuid).FirstOrDefault();

                if (orgMatCat.MatCatDescription == matCatDescription)
                    throw new Exception("There were no changes to the Material Category, so an additional record is not needed.");

                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();

                MaterialCategory newMatCat = new MaterialCategory()
                {
                    Id = Guid.NewGuid(),
                    MatCatDescription = matCatDescription,
                    MatCatId = matCatId,
                    BUILDERInstanceId = (Guid)creds.BUILDERInstanceId,
                    CreatedByUserId = userId,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                orgMatCat.IsActive = false;

                db.MaterialCategories.Add(newMatCat);
                CatalogBusiness.CatalogBusiness cb = new CatalogBusiness.CatalogBusiness(FetchCredentials());

                var cmo = cb.updateMaterialCategory(new CMOMaterialCategory()
                {
                    CatalogDescription = newMatCat.MatCatDescription,
                    MaterialCategoryId = newMatCat.MatCatId
                });

                if (cmo.StatusCode == "200")
                {
                    db.SaveChanges();
                    TempData["UserAlert"] = String.Format("{0} was successfully updated.", matCatDescription);
                    TempData["AlertType"] = "success";
                }
                else
                {
                    throw new Exception(cmo.StatusDetail);
                }

                return RedirectToAction("Index", new { matCatId = matCatId });
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("Could not update {0}: {1}", matCatDescription, ex.Message);
                TempData["AlertType"] = "error";
                return RedirectToAction("Index", new { matCatId = matCatId });
            }
        }

        public ActionResult MaterialCategories_Restore(string guid, int? id)
        {
            try
            {
                Guid matCatGuid = Guid.Parse(guid);
                var matCat = db.MaterialCategories.Where(w => w.Id == matCatGuid).FirstOrDefault();
                var activeMatCat = db.MaterialCategories.Where(w => w.MatCatId == matCat.MatCatId && (bool)w.IsActive).FirstOrDefault();
                activeMatCat.IsActive = false;
                matCat.IsActive = true;

                CatalogBusiness.CatalogBusiness cb = new CatalogBusiness.CatalogBusiness(FetchCredentials());

                var cmo = cb.updateMaterialCategory(new CMOMaterialCategory()
                {
                    CatalogDescription = matCat.MatCatDescription,
                    MaterialCategoryId = matCat.MatCatId
                });

                if (cmo.StatusCode == "200")
                {
                    db.SaveChanges();
                    TempData["UserAlert"] = String.Format("{0} restored to {1}.", matCat.MatCatDescription, matCat.CreatedDate.Value.ToShortDateString());
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

            return RedirectToAction("Index", new { matCatId = id });
        }

        public ActionResult MaterialCategories_Deactivate(int? matCatId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => (bool)w.IsActive && w.AspNetUserId == userId).FirstOrDefault();
                var matCat = db.MaterialCategories.Where(w => w.MatCatId == matCatId && (bool)w.IsActive && creds.BUILDERInstanceId == w.BUILDERInstanceId).FirstOrDefault();
                matCat.IsActive = false;

                db.SaveChanges();

                TempData["UserAlert"] = String.Format("Deactivated Material Category '{0}'. To reactivate or remove entirely, go to 'Manage Inactive Records'.", matCat.MatCatDescription);
                TempData["AlertType"] = "success";
                return RedirectToAction("MaterialCategoriesList", "MaterialCategories");
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("Was unable to deactivate Material Category: {0}", ex.Message);
                TempData["AlertType"] = "error";
                return RedirectToAction("Index", new { id = matCatId });
            }
        }

        [HttpPost]
        public ActionResult Excel_Export_List(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public ActionResult MaterialCategories_RestoreDeactivated(int? matCatId)
        {
            try
            {
                // set the most recent iteration of the system to active
                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId & (bool)w.IsActive).FirstOrDefault();
                var matCat = db.MaterialCategories.Where(w => w.MatCatId == matCatId && w.BUILDERInstanceId == creds.BUILDERInstanceId).OrderByDescending(s => s.CreatedDate).FirstOrDefault();
                matCat.IsActive = true;

                db.SaveChanges();

                TempData["UserAlert"] = String.Format("Successfully restored '{0}' to {1}.", matCat.MatCatDescription, matCat.CreatedDate.Value.ToShortDateString());
                TempData["AlertType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("Was unable to restore the Material Category: {0}.", ex.Message);
                TempData["AlertType"] = "error";
            }

            return RedirectToAction("ManageDeactivated_MaterialCategories", "Application");
        }
    }
}