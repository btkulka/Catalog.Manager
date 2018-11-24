using CatalogData.Classes;
using CatalogData.Classes.CMOs;
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
using ManagerUI.Models.ResyncAnnotations;

namespace ManagerUI.Controllers
{
    public class CatalogController : CMController
    {

        public ActionResult CatalogItemList()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.Identity.GetUserId();
            var cred = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();
            var instance = db.BUILDERInstances.Where(w => w.Id == cred.BUILDERInstanceId).FirstOrDefault();

            ViewBag.LastSync = instance.LastSynced;

            if (instance.LastSynced != null && (DateTime.Now - instance.LastSynced).Value.TotalDays >= 7)
            {
                TempData["UserAlert"] = String.Format("It's been one week since you've last Sync'd with {0}. It's recommended you do this weekly.", instance.Name);
                TempData["AlertType"] = "info";
            }
            else if (instance.LastSynced == null)
            {
                return RedirectToAction("SyncWithBuilder", "Account");
            }

            return View();
        }

        public ActionResult Index(int? cmcId)
        {
            var userId = User.Identity.GetUserId();
            var creds = db.CatalogCredentials.Where(w => (bool)w.IsActive && w.AspNetUserId == userId).FirstOrDefault();
            var catalog = db.CatalogItems.Where(w => (bool)w.IsActive & w.CMCID == cmcId & w.BUILDERInstanceId == creds.BUILDERInstanceId).FirstOrDefault();
            var history = db.CatalogItems.Where(w => (bool)w.IsActive == false & w.CMCID == cmcId & w.BUILDERInstanceId == creds.BUILDERInstanceId).OrderByDescending(o => o.CreationDate).ToList();

            // get references
            var comp = db.Components.Where(w => w.ComponentId == catalog.ComponentId & (bool)w.IsActive & creds.BUILDERInstanceId == w.BUILDERInstanceId).FirstOrDefault();
            var matCat = db.MaterialCategories.Where(w => w.BUILDERInstanceId == creds.BUILDERInstanceId & (bool)w.IsActive & w.MatCatId == catalog.MaterialCategoryId).FirstOrDefault();
            var compType = db.ComponentTypes.Where(w => w.BUILDERInstanceId == creds.BUILDERInstanceId & (bool)w.IsActive & w.CompTypeId == catalog.ComponentTypeId).FirstOrDefault();

            ItemIndexViewModel viewModel = new ItemIndexViewModel()
            {
                CatalogItem = catalog,
                History = history,
                Component = comp,
                MaterialCategory = matCat,
                ComponentType = compType
            };

            return View(viewModel);
        }

        public ActionResult Resync(Guid? builderId)
        {
            var builderInstance = db.BUILDERInstances.Where(w => w.Id == (Guid)builderId).FirstOrDefault();
            return View(builderInstance);
        }

        public ActionResult ResyncResults(Guid? builderId)
        {
            // Setup variables
            ResyncResultViewModel results = new ResyncResultViewModel();
            CatalogBusiness.CatalogBusiness cb = new CatalogBusiness.CatalogBusiness(FetchCredentials());
            var userId = User.Identity.GetUserId();
            var builderInstance = db.BUILDERInstances.Where(w => w.Id == builderId).FirstOrDefault();
            results.BuilderInstance = builderInstance;

            try
            {
                // CATALOG ITEMS ===================================
                var bldrCatalogItems = cb.getAllCMC();
                List<CatalogItemAnnotation> catalogItemResults = new List<CatalogItemAnnotation>();

                foreach (CMOCatalogItem cmo in bldrCatalogItems)
                {
                    var cmc = db.CatalogItems.Where(w => w.CMCID == cmo.CMCID && (bool)w.IsActive).FirstOrDefault();

                    if (cmc != null)
                    {
                        // Do we need to add an updated record?
                        if (CatalogItemAnnotation.HasBeenUpdated(cmc, cmo))
                        {
                            CatalogItem item = new CatalogItem()
                            {
                                AdjFactor = (decimal)cmo.adjFactor,
                                BUILDERInstanceId = (Guid)builderId,
                                CII = (decimal)cmo.CII,
                                CMCID = cmo.CMCID,
                                ComponentId = cmo.ComponentId,
                                ComponentTypeId = cmo.ComponentTypeId,
                                ComponentUiiId = cmo.uiiId,
                                CreatedByUserId = userId,
                                CreationDate = DateTime.Now,
                                IsActive = true,
                                MaterialCategoryId = cmo.MaterialCategoryId,
                                TaskListId = cmo.taskListId,
                                UoM = cmo.UoM,
                                UseAltDesc = cmo.UoM,
                                Id = Guid.NewGuid()
                            };

                            string updateString = CatalogItemAnnotation.GetUpdatedFields(cmc, item);

                            cmc.IsActive = false;
                            db.CatalogItems.Add(item);

                            catalogItemResults.Add(new CatalogItemAnnotation(item, updateString));
                        }
                    }
                    else
                    {
                        // Add a new Catalog Item
                        CatalogItem item = new CatalogItem()
                        {
                            AdjFactor = (decimal)cmo.adjFactor,
                            BUILDERInstanceId = (Guid)builderId,
                            CII = (decimal)cmo.CII,
                            CMCID = cmo.CMCID,
                            ComponentId = cmo.ComponentId,
                            ComponentTypeId = cmo.ComponentTypeId,
                            ComponentUiiId = cmo.uiiId,
                            CreatedByUserId = userId,
                            CreationDate = DateTime.Now,
                            IsActive = true,
                            MaterialCategoryId = cmo.MaterialCategoryId,
                            TaskListId = cmo.taskListId,
                            UoM = cmo.UoM,
                            UseAltDesc = cmo.UoM,
                            Id = Guid.NewGuid()
                        };

                        db.CatalogItems.Add(cmc);
                        catalogItemResults.Add(new CatalogItemAnnotation(item, "Added"));
                    }
                }

                // remove all CatalogItems that are no longer on the BUILDER instance
                var cmcIds = bldrCatalogItems.Select(s => s.CMCID).Distinct();
                var delItems = db.CatalogItems.Where(w => !cmcIds.Contains(w.CMCID) && w.BUILDERInstanceId == builderId).ToList();
                foreach (CatalogItem item in delItems)
                {
                    // we only want one record per Component
                    if (!catalogItemResults.Select(s => s.CatalogItem).ToList().Contains(item))
                    {
                        catalogItemResults.Add(new CatalogItemAnnotation(item, "Deleted"));
                    }

                    db.CatalogItems.Remove(item);
                }

                results.CatalogItemResults = catalogItemResults.OrderBy(o => o.CatalogItem.CMCID).ToList();

                // SYSTEMS =======================================  
                var bldrSystems = cb.getSystems();
                List<SystemAnnotation> systemResults = new List<SystemAnnotation>();

                foreach (CMOSystem cmo in bldrSystems)
                {
                    CatalogSystem sys = db.CatalogSystems.Where(w => w.SystemId == cmo.SystemId && w.BUILDERInstanceId == builderId).FirstOrDefault();

                    if (sys != null)
                    {
                        // Do we need to update?
                        if (SystemAnnotation.HasBeenUpdated(sys, cmo))
                        {
                            CatalogSystem updSys = new CatalogSystem()
                            {
                                BUILDERInstanceId = (Guid)builderId,
                                CreatedByUserId = userId,
                                CreatedDate = DateTime.Now,
                                Id = Guid.NewGuid(),
                                IsActive = true,
                                UiiCode = cmo.UIICode,
                                IsUii = cmo.IsUII,
                                SystemDescription = cmo.Description,
                                SystemId = cmo.SystemId
                            };

                            string updateString = "Updated fields: ";
                            if(sys.SystemDescription != updSys.SystemDescription)
                            {
                                updateString += String.Format(" [SystemDescription {0} => {1}] ", sys.SystemDescription, updSys.SystemDescription);
                            }
                            if(sys.UiiCode != updSys.UiiCode)
                            {
                                updateString += String.Format(" [UiiCode {0} => {1}] ", sys.UiiCode, updSys.UiiCode);
                            }
                            if (sys.IsUii != updSys.IsUii)
                            {
                                updateString += String.Format(" [IsUii {0} => {1}] ", sys.IsUii, updSys.IsUii);
                            }

                            sys.IsActive = false;
                            db.CatalogSystems.Add(updSys);

                            systemResults.Add(new SystemAnnotation(sys, updateString));
                        }
                    }
                    else
                    {
                        // Add new system to DB
                        CatalogSystem newSys = new CatalogSystem()
                        {
                            BUILDERInstanceId = (Guid)builderId,
                            CreatedByUserId = userId,
                            CreatedDate = DateTime.Now,
                            Id = Guid.NewGuid(),
                            IsActive = true,
                            IsUii = cmo.IsUII,
                            SystemDescription = cmo.Description,
                            SystemId = cmo.SystemId,
                            UiiCode = cmo.UIICode
                        };

                        db.CatalogSystems.Add(newSys);
                        systemResults.Add(new SystemAnnotation(newSys, "Added"));
                    }
                }

                // remove all Systems that are no longer on the BUILDER instance
                var systemIds = bldrSystems.Select(s => s.SystemId).Distinct();
                var delSystems = db.CatalogSystems.Where(w => !systemIds.Contains(w.SystemId) && w.BUILDERInstanceId == builderId).ToList();
                foreach(CatalogSystem sys in delSystems)
                {
                    // we only want one record per System
                    if (!systemResults.Select(s => s.System).ToList().Contains(sys))
                    {
                        systemResults.Add(new SystemAnnotation(sys, "Deleted"));
                    }

                    db.CatalogSystems.Remove(sys);
                }

                results.SystemResults = systemResults.OrderBy(o => o.System.SystemId).ToList();

                // COMPONENTS ==================================
                var bldrComponents = cb.getAllComponents();
                List<ComponentAnnotation> componentResults = new List<ComponentAnnotation>();

                foreach (CMOComponent cmo in bldrComponents)
                {
                    Component comp = db.Components.Where(w => w.ComponentId == cmo.ComponentId && w.BUILDERInstanceId == builderId).FirstOrDefault();

                    if (comp != null)
                    {
                        // Do we need to update?
                        if (ComponentAnnotation.HasBeenUpdated(comp, cmo))
                        {
                            Component updComp = new Component()
                            {
                                BUILDERInstanceId = (Guid)builderId,
                                CreatedByUserId = userId,
                                ComponentId = cmo.ComponentId,
                                CreatedDate = DateTime.Now,
                                Description = cmo.Description,
                                Id = Guid.NewGuid(),
                                IsActive = true,
                                IsEquip = cmo.IsEquip,
                                IsUii = cmo.IsUII,
                                SystemId = cmo.SystemId,
                                UiiCode = cmo.UIICode
                            };

                            string updateString = "Updated fields: ";

                            if (comp.IsUii != updComp.IsUii)
                            {
                                updateString += String.Format(" [IsUii {0} => {1}] ", comp.IsUii, updComp.IsUii);
                            }
                            if(comp.IsEquip != updComp.IsEquip)
                            {
                                updateString += String.Format(" [IsEquip {0} => {1}] ", comp.IsEquip, updComp.IsEquip);
                            }
                            if(comp.SystemId != updComp.SystemId)
                            {
                                updateString += String.Format(" [SystemId {0} => {1}] ", comp.SystemId, updComp.SystemId);
                            }
                            if(comp.UiiCode != updComp.UiiCode)
                            {
                                updateString += String.Format(" [UiiCode {0} => {1}] ", comp.UiiCode, updComp.UiiCode);
                            }

                            comp.IsActive = false;
                            db.Components.Add(updComp);

                            componentResults.Add(new ComponentAnnotation(comp, updateString));
                        }
                    }
                    else
                    {
                        // Add new Component to DB
                        Component newComp = new Component()
                        {
                            BUILDERInstanceId = (Guid)builderId,
                            CreatedByUserId = userId,
                            CreatedDate = DateTime.Now,
                            ComponentId = cmo.ComponentId,
                            Id = Guid.NewGuid(),
                            IsActive = true,
                            IsUii = cmo.IsUII,
                            Description = cmo.Description,
                            SystemId = cmo.SystemId,
                            UiiCode = cmo.UIICode
                        };

                        db.Components.Add(newComp);
                        componentResults.Add(new ComponentAnnotation(newComp, "Added"));
                    }
                }

                // remove all Components that are no longer on the BUILDER instance
                var componentIds = bldrComponents.Select(s => s.ComponentId).Distinct();
                var delComponents = db.Components.Where(w => !componentIds.Contains(w.ComponentId) && w.BUILDERInstanceId == builderId).ToList();
                foreach (Component comp in delComponents)
                {
                    // we only want one record per Component
                    if (!componentResults.Select(s => s.Component).ToList().Contains(comp))
                    {
                        componentResults.Add(new ComponentAnnotation(comp, "Deleted"));
                    }

                    db.Components.Remove(comp);
                }

                results.ComponentResults = componentResults.OrderBy(o => o.Component.ComponentId).ToList();

                // SUBCOMPONENTS ===================================
                var bldrSubComponents = cb.getSubComponents();
                List<SubComponentAnnotation> subComponentResults = new List<SubComponentAnnotation>();

                foreach (CMOSubComponent cmo in bldrSubComponents)
                {
                    SubComponent subComp = db.SubComponents.Where(w => w.SubComponentId == cmo.SubComponentId && w.BUILDERInstanceId == builderId).FirstOrDefault();

                    if (subComp != null)
                    {
                        // Do we need to update?
                        if (SubComponentAnnotation.HasBeenUpdated(subComp, cmo))
                        {
                            SubComponent updSubComp = new SubComponent()
                            {
                                BUILDERInstanceId = (Guid)builderId,
                                CreatedByUserId = userId,
                                SubComponentId = cmo.SubComponentId,
                                CreatedDate = DateTime.Now,
                                SubComponentDescription = cmo.Description,
                                Id = Guid.NewGuid(),
                                IsActive = true,
                                SubCompUnitId = cmo.CompUnitsId
                            };

                            string updateString = "Updated fields: ";

                            if (subComp.SubComponentDescription != updSubComp.SubComponentDescription)
                            {
                                updateString += String.Format(" [SubComponentDescription {0} => {1}] ", subComp.SubComponentDescription, updSubComp.SubComponentDescription);
                            }
                            if (subComp.SubCompUnitId != updSubComp.SubCompUnitId)
                            {
                                updateString += String.Format(" [CompUnitId {0} => {1}] ", subComp.SubCompUnitId, updSubComp.SubCompUnitId);
                            }

                            subComp.IsActive = false;
                            db.SubComponents.Add(updSubComp);

                            subComponentResults.Add(new SubComponentAnnotation(subComp, updateString));
                        }
                    }
                    else
                    {
                        // Add new SubComponent to DB
                        SubComponent newSubComp = new SubComponent()
                        {
                            BUILDERInstanceId = (Guid)builderId,
                            CreatedByUserId = userId,
                            CreatedDate = DateTime.Now,
                            SubComponentId = cmo.SubComponentId,
                            Id = Guid.NewGuid(),
                            IsActive = true,
                            SubComponentDescription = cmo.Description,
                            SubCompUnitId = cmo.CompUnitsId
                        };

                        db.SubComponents.Add(newSubComp);
                        subComponentResults.Add(new SubComponentAnnotation(newSubComp, "Added"));
                    }
                }
                
                // remove all SubComponents that are no longer on the BUILDER instance
                var subComponentIds = bldrSubComponents.Select(s => s.SubComponentId).Distinct();
                var delSubComponents = db.SubComponents.Where(w => !subComponentIds.Contains(w.SubComponentId) && w.BUILDERInstanceId == builderId).ToList();
                foreach (SubComponent subComp in delSubComponents)
                {
                    // we only want one record per SubComponent
                    if (!subComponentResults.Select(s => s.SubComponent).ToList().Contains(subComp))
                    {
                        subComponentResults.Add(new SubComponentAnnotation(subComp, "Deleted"));
                    }

                    db.SubComponents.Remove(subComp);
                }

                results.SubComponentResults = subComponentResults.OrderBy(o => o.SubComponent.SubComponentId).ToList();

                // MATERIAL CATEGORIES ==============================
                var bldrMatCats = cb.getMaterialCategories();
                List<MaterialCategoryAnnotation> matCatResults = new List<MaterialCategoryAnnotation>();

                foreach (CMOMaterialCategory cmo in bldrMatCats)
                {
                    MaterialCategory matCat = db.MaterialCategories.Where(w => w.MatCatId == cmo.MaterialCategoryId && w.BUILDERInstanceId == builderId).FirstOrDefault();

                    if (matCat != null)
                    {
                        // Do we need to update?
                        if (MaterialCategoryAnnotation.HasBeenUpdated(matCat, cmo))
                        {
                            MaterialCategory updMatCat = new MaterialCategory()
                            {
                                BUILDERInstanceId = (Guid)builderId,
                                CreatedByUserId = userId,
                                MatCatId = cmo.MaterialCategoryId,
                                CreatedDate = DateTime.Now,
                                MatCatDescription = cmo.CatalogDescription,
                                Id = Guid.NewGuid(),
                                IsActive = true
                            };

                            string updateString = "Updated fields: ";

                            if (matCat.MatCatDescription != updMatCat.MatCatDescription)
                            {
                                updateString += String.Format(" [Description {0} => {1}] ", matCat.MatCatDescription, updMatCat.MatCatDescription);
                            }
                            
                            matCat.IsActive = false;
                            db.MaterialCategories.Add(updMatCat);

                            matCatResults.Add(new MaterialCategoryAnnotation(matCat, updateString));
                        }
                    }
                    else
                    {
                        // Add new MaterialCategory to DB
                        MaterialCategory newMatCat = new MaterialCategory()
                        {
                            BUILDERInstanceId = (Guid)builderId,
                            CreatedByUserId = userId,
                            CreatedDate = DateTime.Now,
                            MatCatId = cmo.MaterialCategoryId,
                            Id = Guid.NewGuid(),
                            IsActive = true,
                            MatCatDescription = cmo.CatalogDescription
                        };

                        db.MaterialCategories.Add(newMatCat);
                        matCatResults.Add(new MaterialCategoryAnnotation(newMatCat, "Added"));
                    }
                }

                // remove all Material Categories that are no longer on the BUILDER instance
                var matCatIds = bldrMatCats.Select(s => s.MaterialCategoryId).Distinct();
                var delMatCats = db.MaterialCategories.Where(w => !matCatIds.Contains(w.MatCatId) && w.BUILDERInstanceId == builderId).ToList();
                foreach (MaterialCategory matCat in delMatCats)
                {
                    // we only want one record per Material Category
                    if (!matCatResults.Select(s => s.MaterialCategory).ToList().Contains(matCat))
                    {
                        matCatResults.Add(new MaterialCategoryAnnotation(matCat, "Deleted"));
                    }

                    db.MaterialCategories.Remove(matCat);
                }

                results.MatCatResults = matCatResults.OrderBy(o => o.MaterialCategory.MatCatId).ToList();

                // COMPONENT TYPES ============================
                var bldrCompTypes = cb.getComponentTypes();
                List<ComponentTypeAnnotation> compTypeResults = new List<ComponentTypeAnnotation>();

                foreach (CMOComponentType cmo in bldrCompTypes)
                {
                    ComponentType compType = db.ComponentTypes.Where(w => w.CompTypeId == cmo.ComponentTypeId && w.BUILDERInstanceId == builderId).FirstOrDefault();

                    if (compType != null)
                    {
                        // Do we need to update?
                        if (ComponentTypeAnnotation.HasBeenUpdated(compType, cmo))
                        {
                            ComponentType updCompType = new ComponentType()
                            {
                                BUILDERInstanceId = (Guid)builderId,
                                CreatedByUserId = userId,
                                CompTypeId = cmo.ComponentTypeId,
                                CreatedDate = DateTime.Now,
                                CompTypeDescription = cmo.Description,
                                Id = Guid.NewGuid(),
                                IsActive = true
                            };

                            string updateString = "Updated fields: ";

                            if (compType.CompTypeDescription != updCompType.CompTypeDescription)
                            {
                                updateString += String.Format(" [Description {0} => {1}] ", compType.CompTypeDescription, updCompType.CompTypeDescription);
                            }

                            compType.IsActive = false;
                            db.ComponentTypes.Add(updCompType);

                            compTypeResults.Add(new ComponentTypeAnnotation(compType, updateString));
                        }
                    }
                    else
                    {
                        // Add new ComponentType to DB
                        ComponentType newCompType = new ComponentType()
                        {
                            BUILDERInstanceId = (Guid)builderId,
                            CreatedByUserId = userId,
                            CreatedDate = DateTime.Now,
                            CompTypeId = cmo.ComponentTypeId,
                            Id = Guid.NewGuid(),
                            IsActive = true,
                            CompTypeDescription = cmo.Description
                        };

                        db.ComponentTypes.Add(newCompType);
                        compTypeResults.Add(new ComponentTypeAnnotation(newCompType, "Added"));
                    }
                }

                // remove all Material Categories that are no longer on the BUILDER instance
                var compTypeIds = bldrCompTypes.Select(s => s.ComponentTypeId).Distinct();
                var delCompTypes = db.ComponentTypes.Where(w => !compTypeIds.Contains(w.CompTypeId) && w.BUILDERInstanceId == builderId).ToList();
                foreach (ComponentType compType in delCompTypes)
                {
                    // we only want one record per Material Category
                    if (!compTypeResults.Select(s => s.ComponentType).ToList().Contains(compType))
                    {
                        compTypeResults.Add(new ComponentTypeAnnotation(compType, "Deleted"));
                    }

                    db.ComponentTypes.Remove(compType);
                }

                results.ComponentTypeResults = compTypeResults.OrderBy(o => o.ComponentType.CompTypeId).ToList();

                // update instance
                results.BuilderInstance.LastSynced = DateTime.Now;

                db.SaveChanges();

                TempData["UserAlert"] = String.Format("Successfully sync'd with {0}.", builderInstance.Name);
                TempData["AlertType"] = "success";
                ViewBag.ResyncResults = results;

                return View(results);
            }catch(Exception ex)
            {
                TempData["UserAlert"] = String.Format("An error occurred while attempting to sync with {0}: {1}", builderInstance.Name, ex.Message);
                TempData["AlertType"] = "error";

                return View(results);
            }
        }

        public ActionResult CatalogItems_Read(DataSourceRequest request)
        {
            var userId = User.Identity.GetUserId();
            var creds = db.CatalogCredentials.Where(w => (bool)w.IsActive && w.AspNetUserId == userId).FirstOrDefault();
            List<CatalogItem> items = db.CatalogItems.Where(w => (bool)w.IsActive && w.BUILDERInstanceId == creds.BUILDERInstanceId).OrderBy(o => o.CMCID).ToList();

            DataSourceResult result = items.ToDataSourceResult(request, item => new CatalogItemViewModel(){
                CMCID = item.CMCID,
                ComponentId = item.ComponentId,
                ComponentName = db.Components.Where(w => w.ComponentId == item.ComponentId).FirstOrDefault().Description,
                MaterialCategoryId = item.MaterialCategoryId,
                MaterialCategoryName = db.MaterialCategories.Where(w => w.MatCatId == item.MaterialCategoryId).FirstOrDefault().MatCatDescription,
                ComponentTypeId = item.ComponentTypeId,
                ComponentTypeDesc = db.ComponentTypes.Where(w => w.CompTypeId == item.ComponentTypeId).FirstOrDefault().CompTypeDescription,
                CII = item.CII,
                AdjFactor = item.AdjFactor,
                TaskListId = item.TaskListId,
                UoM = item.UoM,
                UseAltDesc = item.UseAltDesc
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CatalogItems_Deactivate(int? cmcid){
            try {
                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId & (bool)w.IsActive).FirstOrDefault();
                var item = db.CatalogItems.Where(w => w.CMCID == cmcid && (bool)w.IsActive & w.BUILDERInstanceId == creds.BUILDERInstanceId).FirstOrDefault();
                item.IsActive = false;

                db.SaveChanges();

                TempData["UserAlert"] = String.Format("Deactivated CatalogItem '{0}'. To reactivate or remove entirely, go to 'Manage Inactive Records'.", item.CMCID);
                TempData["AlertType"] = "success";
                return RedirectToAction("CatalogItemList", "Catalog");
            }
            catch (Exception ex) {
                TempData["UserAlert"] = String.Format("Was unable to deactivate Catalog Item: {0}", ex.Message);
                TempData["AlertType"] = "error";
                return RedirectToAction("Index", "Catalog", new { cmcId = cmcid });
            }
        }

        public ActionResult CatalogItems_Create(DataSourceRequest request)
        {
            return null;
        }

        public ActionResult CatalogItems_Delete(DataSourceRequest request)
        {
            return null;
        }

        public ActionResult CatalogItems_Update(DataSourceRequest request)
        {
            return null;
        }

        public ActionResult SyncWithBuilder()
        {
            try
            {
                // Fetch variables needed for Sync
                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();
                var builderId = db.BUILDERInstances.Where(w => w.Id == creds.BUILDERInstanceId).FirstOrDefault().Id;

                // Begin by fetching all CatalogItems
                CatalogBusiness.CatalogBusiness cb = new CatalogBusiness.CatalogBusiness(new CatalogCredentials(creds.BUILDERInstance.RemoteServiceAddress, creds.BuilderUn, creds.BuilderPw));
                var cmc = cb.getAllCMC();

                foreach (CMOCatalogItem cmo in cmc)
                {
                    CatalogItem item = new CatalogItem()
                    {
                        Id = Guid.NewGuid(),
                        AdjFactor = (decimal)cmo.adjFactor,
                        BUILDERInstanceId = builderId,
                        CII = Convert.ToDecimal(cmo.CII),
                        CMCID = cmo.CMCID,
                        ComponentId = cmo.ComponentId,
                        ComponentTypeId = cmo.ComponentTypeId,
                        ComponentUiiId = cmo.uiiId,
                        CreationDate = DateTime.Now,
                        IsActive = true,
                        MaterialCategoryId = cmo.MaterialCategoryId,
                        TaskListId = cmo.taskListId,
                        UoM = cmo.UoM,
                        UseAltDesc = cmo.useAltDesc,
                        CreatedByUserId = userId
                    };

                    db.CatalogItems.Add(item);
                }


                // Retrieve systems
                var systems = cb.getSystems();

                foreach (CMOSystem system in systems)
                {
                    CatalogSystem sys = new CatalogSystem()
                    {
                        Id = Guid.NewGuid(),
                        CreatedByUserId = userId,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsUii = system.IsUII,
                        SystemDescription = system.Description,
                        SystemId = system.SystemId,
                        UiiCode = system.UIICode,
                        BUILDERInstanceId = builderId
                    };

                    db.CatalogSystems.Add(sys);
                }

                // Retrieve all Components
                var components = cb.getAllComponents();

                foreach (CMOComponent component in components)
                {
                    Component comp = new Component()
                    {
                        Id = Guid.NewGuid(),
                        BUILDERInstanceId = builderId,
                        SystemId = component.SystemId,
                        ComponentId = component.ComponentId,
                        CreatedByUserId = userId,
                        CreatedDate = DateTime.Now,
                        Description = component.Description,
                        IsEquip = component.IsEquip,
                        IsUii = component.IsUII,
                        UiiCode = component.UIICode,
                        IsActive = true
                    };

                    db.Components.Add(comp);
                }

                // Retrieve all ComponentTypes
                var compTypes = cb.getComponentTypes();

                foreach (CMOComponentType compType in compTypes)
                {
                    ComponentType type = new ComponentType()
                    {
                        Id = Guid.NewGuid(),
                        BUILDERInstanceId = builderId,
                        CompTypeDescription = compType.Description,
                        CompTypeId = compType.ComponentTypeId,
                        CreatedByUserId = userId,
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    };

                    db.ComponentTypes.Add(type);
                }

                var materialCategories = cb.getMaterialCategories();
                foreach (CMOMaterialCategory matCat in materialCategories)
                {
                    MaterialCategory cat = new MaterialCategory()
                    {
                        Id = Guid.NewGuid(),
                        BUILDERInstanceId = builderId,
                        CreatedByUserId = userId,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        MatCatDescription = matCat.CatalogDescription,
                        MatCatId = matCat.MaterialCategoryId
                    };

                    db.MaterialCategories.Add(cat);
                }

                var subComponents = cb.getSubComponents();
                foreach (CMOSubComponent subComp in subComponents)
                {
                    SubComponent sc = new SubComponent()
                    {
                        Id = Guid.NewGuid(),
                        BUILDERInstanceId = builderId,
                        CreatedByUserId = userId,
                        CreatedDate = DateTime.Now,
                        SubComponentDescription = subComp.Description,
                        SubComponentId = subComp.SubComponentId,
                        SubCompUnitId = subComp.CompUnitsId,
                        IsActive = true
                    };

                    db.SubComponents.Add(sc);
                }

                var builderInstance = db.BUILDERInstances.Where(w => w.Id == builderId).FirstOrDefault();
                builderInstance.LastSynced = DateTime.Now;

                db.SaveChanges();

                TempData["UserAlert"] = String.Format("Successfully sync'd up with {0}'s Catalog.", builderInstance.Name);
                TempData["AlertType"] = "success";
                return RedirectToAction("Index", "Catalog");
            }catch(Exception ex)
            {
                TempData["UserAlert"] = String.Format("An error occurred while attempting to sync with an external BUILDER catalog: {0}", ex.Message);
                TempData["AlertType"] = "error";
                return View();
            }
        }
    }
}