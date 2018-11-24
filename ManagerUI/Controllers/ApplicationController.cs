using CatalogData;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ManagerUI.Controllers
{
    public class ApplicationController : CMController
    {
        // GET: Application
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageDeactivated_CatalogItems()
        {
            var userId = User.Identity.GetUserId();
            var creds = db.CatalogCredentials.Where(w => (bool)w.IsActive && w.AspNetUserId == userId).FirstOrDefault();
            List<CatalogItem> deactivatedCatalogItems = new List<CatalogItem>();

            // Retrieve all inactive Systems
            var inactiveItems = db.CatalogItems.Where(w => (bool)w.IsActive == false && w.BUILDERInstanceId == creds.BUILDERInstanceId).ToList();

            // Filter out records with one active record
            foreach (CatalogItem item in inactiveItems) {
                if (db.CatalogItems.Where(w => w.CMCID == item.CMCID && (bool)w.IsActive == true).Count() == 0) {
                    // is this system already in the list?
                    if (deactivatedCatalogItems.Where(w => w.ComponentId == item.CMCID).Count() == 0) {
                        deactivatedCatalogItems.Add(item);
                    }
                }
            }

            ViewBag.DeactivatedCatalogItems = deactivatedCatalogItems;

            return View();
        }

        public ActionResult ManageDeactivated_Components()
        {
            List<Component> deactivatedComponents = new List<Component>();

            // Retrieve all inactive Systems
            var inactiveComponents = db.Components.Where(w => (bool)w.IsActive == false).ToList();

            // Filter out records with one active record
            foreach (Component comp in inactiveComponents)
            {
                if (db.Components.Where(w => w.ComponentId == comp.ComponentId && (bool)w.IsActive == true).Count() == 0)
                {
                    // is this system already in the list?
                    if (deactivatedComponents.Where(w => w.ComponentId == comp.ComponentId).Count() == 0)
                    {
                        deactivatedComponents.Add(comp);
                    }
                }
            }

            ViewBag.DeactivatedComponents = deactivatedComponents;
            
            return View();
        }

        public ActionResult ManageDeactivated_ComponentTypes()
        {
            List<ComponentType> deactivatedComponentTypes = new List<ComponentType>();

            // Retrieve all inactive Systems
            var inactiveComponentTypes = db.ComponentTypes.Where(w => (bool)w.IsActive == false).ToList();

            // Filter out records with one active record
            foreach (ComponentType compType in inactiveComponentTypes)
            {
                if (db.ComponentTypes.Where(w => w.CompTypeId == compType.CompTypeId && (bool)w.IsActive == true).Count() == 0)
                {
                    // is this system already in the list?
                    if (deactivatedComponentTypes.Where(w => w.CompTypeId == compType.CompTypeId).Count() == 0)
                    {
                        deactivatedComponentTypes.Add(compType);
                    }
                }
            }

            ViewBag.DeactivatedComponentTypes = deactivatedComponentTypes;

            return View();
        }

        public ActionResult ManageDeactivated_MaterialCategories()
        {
            List<MaterialCategory> deactivatedMaterialCategories = new List<MaterialCategory>();

            // Retrieve all inactive Systems
            var inactiveMatCats = db.MaterialCategories.Where(w => (bool)w.IsActive == false).ToList();

            // Filter out records with one active record
            foreach (MaterialCategory matCat in inactiveMatCats)
            {
                if (db.MaterialCategories.Where(w => w.MatCatId == matCat.MatCatId && (bool)w.IsActive == true).Count() == 0)
                {
                    // is this system already in the list?
                    if (deactivatedMaterialCategories.Where(w => w.MatCatId == matCat.MatCatId).Count() == 0)
                    {
                        deactivatedMaterialCategories.Add(matCat);
                    }
                }
            }

            ViewBag.DeactivatedMaterialCategories = deactivatedMaterialCategories;

            return View();
        }

        public ActionResult ManageDeactivated_Systems()
        {
            List<CatalogSystem> deactivatedSystems = new List<CatalogSystem>();

            // Retrieve all inactive Systems
            var inactiveSystems = db.CatalogSystems.Where(w => (bool)w.IsActive == false).ToList();

            // Filter out records with one active record
            foreach(CatalogSystem sys in inactiveSystems)
            {
                if(db.CatalogSystems.Where(w => w.SystemId == sys.SystemId && (bool)w.IsActive == true).Count() == 0)
                {
                    // is this system already in the list?
                    if(deactivatedSystems.Where(w => w.SystemId == sys.SystemId).Count() == 0)
                    {
                        deactivatedSystems.Add(sys);
                    }                   
                }
            }


            ViewBag.DeactivatedSystems = deactivatedSystems;

            return View();
        }

        public ActionResult ManageDeactivated_SubComponents()
        {
            List<SubComponent> deactivatedSubComponents = new List<SubComponent>();

            // Retrieve all inactive Systems
            var inactiveSubComponents = db.SubComponents.Where(w => (bool)w.IsActive == false).ToList();

            // Filter out records with one active record
            foreach (SubComponent subComp in inactiveSubComponents)
            {
                if (db.SubComponents.Where(w => w.SubComponentId == subComp.SubComponentId && (bool)w.IsActive == true).Count() == 0)
                {
                    // is this system already in the list?
                    if (deactivatedSubComponents.Where(w => w.SubComponentId == subComp.SubComponentId).Count() == 0)
                    {
                        deactivatedSubComponents.Add(subComp);
                    }
                }
            }

            ViewBag.DeactivatedSubComponents = deactivatedSubComponents;

            return View();
        }

        public ActionResult ManageBuilderInstances()
        {
            List<BUILDERInstance> instances = new List<BUILDERInstance>();
            
            var userId = User.Identity.GetUserId();
            var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId).OrderByDescending(s => s.IsActive).ToList();

            // Retrieve a list of all instances that the user has credentials for
            foreach(CatalogCredential cred in creds)
            {
                if (!instances.Contains(cred.BUILDERInstance) && cred.BUILDERInstance.FlagDeleted == false)
                {
                    instances.Add(cred.BUILDERInstance);
                }
            }

            // get list of all other available instances
            List<SelectListItem> otherInstances = new List<SelectListItem>();
            foreach(BUILDERInstance instance in db.BUILDERInstances.Where(w => w.FlagDeleted == false))
            {
                if (!instances.Contains(instance))
                {
                    otherInstances.Add(new SelectListItem() { Text = instance.Name, Value = instance.Id.ToString() });
                }
            }

            // Set ViewBag variables
            ViewBag.BUILDERInstances = instances;
            ViewBag.DefaultInstanceId = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault().BUILDERInstanceId;
            ViewBag.UserId = userId;
            ViewBag.OtherInstances = otherInstances;

            return View();
        }

        public ActionResult BuilderInstances_Update(FormCollection details)
        {
            try
            {
                var builderId = Guid.Parse(details["BuilderInstanceId"]);
                var builderName = details["BuilderInstanceName"];
                var remoteServiceAddress = details["RemoteServiceAddress"];
                var builderUn = details["BuilderUsername"];
                var builderPw = details["BuilderPassword"];

                if (builderUn.Trim() == "")
                {
                    throw new Exception("Your BUILDER Username cannot be blank.");
                }

                if (builderPw.Trim() == "")
                {
                    throw new Exception("Your BUILDER Password cannot be blank.");
                }

                if (builderName.Trim() == "")
                {
                    throw new Exception("The new BUILDER name cannot be blank.");
                }

                if (remoteServiceAddress.Trim() == "")
                {
                    throw new Exception("The Remote Service Address cannot be blank.");
                }

                // Update BUILDER instance
                var builderInstance = db.BUILDERInstances.Where(w => w.Id == builderId).FirstOrDefault();
                builderInstance.Name = builderName;
                builderInstance.RemoteServiceAddress = remoteServiceAddress;

                // update associated creds
                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => w.BUILDERInstanceId == builderId && w.AspNetUserId == userId).FirstOrDefault();
                creds.BuilderPw = builderPw;
                creds.BuilderUn = builderUn;

                db.SaveChanges();

                TempData["UserAlert"] = String.Format("BUILDER Instance '{0}' successfully updated.", builderName);
                TempData["AlertType"] = "success";
            }catch(Exception ex)
            {
                TempData["UserAlert"] = String.Format("There was an error when updating the BUILDER Instance: {0}", ex.Message);
                TempData["AlertType"] = "error";
            }

            return RedirectToAction("ManageBuilderInstances");
        }

        public ActionResult BuilderInstances_Create(FormCollection details)
        {
            try
            {
                var builderName = details["BuilderInstanceName"];
                var remoteServiceAddress = details["RemoteServiceAddress"];
                var builderUn = details["BuilderUsername"];
                var builderPw = details["BuilderPassword"];

                if (builderUn.Trim() == "")
                {
                    throw new Exception("Your BUILDER Username cannot be blank.");
                }

                if (builderPw.Trim() == "")
                {
                    throw new Exception("Your BUILDER Password cannot be blank.");
                }
                
                if (builderName.Trim() == "")
                {
                    throw new Exception("The new BUILDER name cannot be blank.");
                }

                if(remoteServiceAddress.Trim() == "")
                {
                    throw new Exception("The Remote Service Address cannot be blank.");
                }

                // Create new instance
                BUILDERInstance instance = new BUILDERInstance();
                instance.Id = Guid.NewGuid();
                instance.Name = builderName;
                instance.RemoteServiceAddress = remoteServiceAddress;
                instance.FlagDeleted = false;

                // Create new credentials
                CatalogCredential cred = new CatalogCredential();
                cred.Id = Guid.NewGuid();
                cred.BuilderUn = builderUn;
                cred.BuilderPw = builderPw;
                cred.BUILDERInstanceId = instance.Id;
                cred.AspNetUserId = User.Identity.GetUserId();

                // Add to DB
                db.BUILDERInstances.Add(instance);
                db.CatalogCredentials.Add(cred);
                db.SaveChanges();

                TempData["UserAlert"] = String.Format("BUILDER Instance '{0}' successfully created.", builderName);
                TempData["AlertType"] = "success";
            }catch(Exception ex)
            {
                TempData["UserAlert"] = String.Format("An error occurred while trying to create new BUILDER instance: {0}", ex.Message);
                TempData["AlertType"] = "error";
            }

            return RedirectToAction("ManageBuilderInstances");
        }

        public ActionResult BuilderInstances_Deactivate(Guid? builderId)
        {
            try
            {
                var instance = db.BUILDERInstances.Where(w => w.Id == builderId).FirstOrDefault();
                var userId = User.Identity.GetUserId();

                // Any User that has this as their only BUILDER Instance will prevent this deactivation
                foreach (AspNetUser user in db.CatalogCredentials.Where(w => w.BUILDERInstanceId == builderId).Select(s => s.AspNetUser).ToList())
                {
                    if (db.CatalogCredentials.Where(w => w.AspNetUserId == user.Id && w.BUILDERInstanceId != builderId && w.BUILDERInstance.FlagDeleted == false).Count() == 0)
                    {
                        if (user.Id == userId)
                        {
                            throw new Exception("You cannot deactivate your only active instance of BUILDER.");
                        }
                        else
                        {
                            throw new Exception("A user is dependent on this BUILDER Instance.");
                        }
                    }
                }

                // Select new active credentials for any User that has this instance selected
                foreach(CatalogCredential cred in db.CatalogCredentials.Where(w => w.BUILDERInstanceId == builderId).ToList())
                {
                    if ((bool)cred.IsActive)
                    {
                        cred.IsActive = false;
                        var altCred = db.CatalogCredentials.Where(w => w.AspNetUserId == cred.AspNetUserId && w.Id != cred.Id).FirstOrDefault();
                        altCred.IsActive = true;
                    }
                }

                // Deactivate BUILDER Instance
                instance.FlagDeleted = true;

                db.SaveChanges();

                TempData["UserAlert"] = String.Format("Deactived BUILDER Instance '{0}'.", instance.Name);
                TempData["AlertType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("An error occurred when attempting to deactivate BUILDER Instance: {0}", ex.Message);
                TempData["AlertType"] = "error";
            }

            return RedirectToAction("ManageBuilderInstances");
        }

        public ActionResult BuilderInstances_Select(Guid? builderId)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId).ToList();
                string builderName = "";

                // Set active credentials, deactive all others
                foreach(CatalogCredential cred in creds)
                {
                    if(cred.BUILDERInstanceId == builderId)
                    {
                        cred.IsActive = true;
                        builderName = cred.BUILDERInstance.Name;
                    }
                    else
                    {
                        cred.IsActive = false;
                    }
                }

                db.SaveChanges();

                // Swap o
                Session["BuilderInstance"] = builderName;

                TempData["UserAlert"] = String.Format("Changed BUILDER Instance to {0}.", builderName);
                TempData["AlertType"] = "success";
            }catch(Exception ex)
            {
                TempData["UserAlert"] = String.Format("There was an error when attempting to change BUILDER Instances: {0}.", ex.Message);
                TempData["AlertType"] = "error";
            }
            return RedirectToAction("ManageBuilderInstances");
        }

        public ActionResult BuilderInstances_Link(FormCollection details)
        {
            try
            {
                var builderId = Guid.Parse(details["BuilderInstanceId"]);
                var builderUn = details["BuilderUsername"];
                var builderPw = details["BuilderPassword"];
                var userId = User.Identity.GetUserId();

                if(builderUn.Trim() == "")
                {
                    throw new Exception("Your BUILDER Username cannot be blank.");
                }

                if(builderPw.Trim() == "")
                {
                    throw new Exception("Your BUILDER Password cannot be blank.");
                }

                // Create credentials
                CatalogCredential cred = new CatalogCredential()
                {
                    Id = Guid.NewGuid(),
                    BuilderUn = builderUn,
                    BuilderPw = builderPw,
                    AspNetUserId = userId,
                    BUILDERInstanceId = builderId
                };

                db.CatalogCredentials.Add(cred);
                db.SaveChanges();

                TempData["UserAlert"] = String.Format("Successfully linked '{0}'.", db.BUILDERInstances.Where(w => w.Id == builderId).FirstOrDefault().Name);
                TempData["AlertType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["UserAlert"] = String.Format("An error occurred while attempting to link an existing BUILDER instance: {0}", ex.Message);
                TempData["AlertType"] = "error";
           }

            return RedirectToAction("ManageBuilderInstances");
        }
    }
}