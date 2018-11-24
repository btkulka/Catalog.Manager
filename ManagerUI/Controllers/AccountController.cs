using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CatalogData;
using CatalogData.Classes;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using ManagerUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace ManagerUI.Controllers
{
    public class AccountController : CMController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            // Fetch user
            var userId = User.Identity.GetUserId();
            AspNetUser user = db.AspNetUsers.Where(w => w.Id == userId).FirstOrDefault();

            return View(user);
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult SyncWithBuilder()
        {
            return View();
        }

        public ActionResult Logout()
        {
            SignInManager.AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        public async Task<ActionResult> Authenticate(FormCollection details)
        {
            // Set variables
            var userName = details["Username"];
            var password = details["Password"];
            try
            {
                // Ensure validity of input
                if (userName == "")
                    throw new Exception("Please enter username.");
                if (password == "")
                    throw new Exception("Please enter password.");

                PasswordHasher hasher = new PasswordHasher();
                var passwordHash = hasher.HashPassword(password);

                // fetch user
                AspNetUser user = db.AspNetUsers.Where(w => w.UserName == userName).FirstOrDefault();

                if (user == null)
                    throw new Exception("Invalid username/password combination.");
                else
                {
                    SignInStatus result = await SignInManager.PasswordSignInAsync(user.UserName, password, true, shouldLockout: false);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            RedirectToAction("CatalogItemList", "Catalog");
                            break;
                        case SignInStatus.Failure:
                            throw new Exception("Invalid username/password.");
                        case SignInStatus.LockedOut:
                            throw new Exception("User is currently locked out. Contact your administrator.");
                        default:
                            throw new Exception("User was unable to be signed in.");
                    }
                }
                return RedirectToAction("CatalogItemList", "Catalog");
            }catch(Exception ex)
            {
                // Error, return to login
                TempData["UserAlert"] = ex.Message;
                TempData["AlertType"] = "error";
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Register()
        {
            var instances = db.BUILDERInstances.ToList();
            List<SelectListItem> instanceList = new List<SelectListItem>();

            foreach (BUILDERInstance instance in instances)
            {
                instanceList.Add(new SelectListItem() { Text = instance.Name, Value = instance.Id.ToString() });
            }

            ViewBag.Instances = instanceList;

            return View();
        }

        /*  NAME:   CreateUser
         *  DESC:   Method called from the form on Register.cshtml. Creates a new AspNetUser,
         *          as well as an associated CatalogCredentials object. Returns to the Account
         *          Index on success.
         *  PARAMS: {0} details - a FormCollection of all User Creation details passed from Register
         *  RETURN: Redirects to Account/Index on Success, or returns to Account/Register on fail.
         */
        public ActionResult CreateUser(FormCollection details)
        {
            // Set variables
            var userName = details["UserName"];
            var builderUn = details["BUILDERUsername"];
            var builderId = details["Instance"];
            var password = details["Password"];
            var builderPassword = details["BUILDERPassword"];
            var email = details["Email"];

            try
            {
                // Do we need to create the first BUILDER Instance?
                if(builderId == null)
                {
                    var builderName = details["InstanceName"];
                    var remoteAddress = details["ServiceAddress"];

                    if (builderName == null)
                        throw new Exception("You must create a new BUILDER Instance. Please enter a name for it.");
                    else if (db.BUILDERInstances.Where(w => w.Name == builderName).Count() > 0)
                        throw new Exception("A BUILDER instance with this name already exists for Catalog Manager.");
                    if (remoteAddress == null)
                        throw new Exception("You must create a new BUILDER Instance. Please enter the Remote Service URL for it.");
                    else if (db.BUILDERInstances.Where(w => w.RemoteServiceAddress == remoteAddress).Count() > 0)
                        throw new Exception("A BUILDER instance with this remote address already exists for Catalog Manager.");

                    BUILDERInstance instance = new BUILDERInstance()
                    {
                        RemoteServiceAddress = remoteAddress,
                        Name = builderName,
                        FlagDeleted = false,
                        LastSynced = null,
                        Id = Guid.NewGuid()
                    };

                    db.BUILDERInstances.Add(instance);
                    db.SaveChanges();

                    builderId = instance.Id.ToString();
                }

                // Check validity of passed fields
                if (userName == "")
                    throw new Exception("The 'Username' field cannot be blank.");
                if (builderUn == "")
                    throw new Exception("The 'BUILDER Username' field cannot be blank.");
                if (password == "")
                    throw new Exception("The 'Password' field cannot be blank.");
                if (builderPassword == "")
                    throw new Exception("The 'BUILDER Password' field cannot be blank.");
                if (email == "")
                    throw new Exception("The 'Email' field cannot be blank.");

                // Ensure passwords match
                if (password != details["ConfirmPassword"])
                    throw new Exception("Your password confirmation does not match your password. Please try again.");
                if (builderPassword != details["ConfirmBUILDERPassword"])
                    throw new Exception("Your BUILDER account password confirmation does not match your BUILDER password. Please try again.");

                // Hash passwords
                PasswordHasher hasher = new PasswordHasher();
                string builderHashedPass = hasher.HashPassword(builderPassword);

                // Initialize AspNetUser manager classes and create
                var user = new ApplicationUser() { UserName = userName , Email = email};
                IdentityResult result = UserManager.Create(user, password);

                if (result.Succeeded)
                {
                    // Create catalog credentials record
                    CatalogCredential cred = new CatalogCredential()
                    {
                        Id = Guid.NewGuid(),
                        AspNetUserId = user.Id,
                        BuilderPw = builderHashedPass,
                        BuilderUn = builderUn,
                        BUILDERInstanceId = Guid.Parse(builderId),
                        IsActive = true // first cred will be selected by default
                    };

                    db.CatalogCredentials.Add(cred);
                    db.SaveChanges();

                    // Sign the user in
                    SignInManager.SignIn<ApplicationUser, string>(user, true, true);

                    // Set success message
                    TempData["UserAlert"] = String.Format("User {0} was successfully created.", userName);
                    TempData["AlertType"] = "success";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Set error message
                    TempData["UserAlert"] = String.Format("An error occurred while trying to create User {0}: {1}", userName, result.Errors.FirstOrDefault());
                    TempData["AlertType"] = "error";
                    return RedirectToAction("Register");
                }
            }catch(Exception ex)
            {
                // Set error message and return to Register
                TempData["UserAlert"] = String.Format("An error occurred while trying to create User {0}: {1}", userName, ex.Message);
                TempData["AlertType"] = "error";
                return RedirectToAction("Register");
            }
        }

        public ActionResult BUILDERCreds_Read([DataSourceRequest]DataSourceRequest request)
        {
            var userId = User.Identity.GetUserId();
            List<CatalogCredential> creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId).ToList();

            DataSourceResult result = creds.ToDataSourceResult(request, cred => new CredentialViewModel{
                Id = cred.Id,
                BuilderUn = cred.BuilderUn,
                BuilderPw = cred.BuilderPw,
                BuilderInstanceName = cred.BUILDERInstance.Name,
                IsActive = cred.IsActive ?? false,
                AspNetUserId = cred.AspNetUserId
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BUILDERCreds_Create([DataSourceRequest]DataSourceRequest request, CatalogCredential cred)
        {
            try
            {
                PasswordHasher hasher = new PasswordHasher();
                var entity = new CatalogCredential
                {
                    Id = Guid.NewGuid(),
                    AspNetUserId = User.Identity.GetUserId(),
                    BuilderUn = cred.BuilderUn,
                    BuilderPw = hasher.HashPassword(cred.BuilderPw),
                    BUILDERInstanceId = cred.BUILDERInstanceId,
                    IsActive = cred.IsActive ?? false
                };

                db.CatalogCredentials.Add(entity);
                db.SaveChanges();
                cred.Id = entity.Id;

                // We need to set all other Credentials to inactive for this user
                if ((bool)entity.IsActive)
                {
                    var userCreds = db.CatalogCredentials.Where(w => w.AspNetUserId == entity.AspNetUserId && w.Id != cred.Id).ToList();
                    foreach (CatalogCredential c in userCreds)
                    {
                        c.IsActive = false;
                    }
                    db.SaveChanges();
                }

                return Json(new[] { cred }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                TempData["UserAlert"] = "CatalogManager was unable to create the BUILDER Credentials: " + ex.Message;
                TempData["AlertType"] = "error";
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BUILDERCreds_Update([DataSourceRequest]DataSourceRequest request, CatalogCredential cred)
        {
            var entity = db.CatalogCredentials.Where(w => cred.Id == w.Id).FirstOrDefault();

            entity.BuilderUn = cred.BuilderUn;
            entity.BuilderPw = cred.BuilderPw;
            entity.BUILDERInstanceId = cred.BUILDERInstanceId;
            entity.IsActive = cred.IsActive;

            // We need to set all other Credentials to inactive for this user
            var userCreds = db.CatalogCredentials.Where(w => w.AspNetUserId == cred.AspNetUserId).ToList();
            if ((bool)cred.IsActive)
            {
                foreach(CatalogCredential c in userCreds.Where(w => w.Id != cred.Id))
                {
                    c.IsActive = false;
                }
            }else if(!(bool)entity.IsActive && userCreds.Where(w => w.Id != cred.Id && (bool)w.IsActive).Count() == 0)
            {
                // the currently selected cred is being saved as inactive, choose the first for default
                var newDefault = userCreds.Where(w => w.Id != cred.Id).FirstOrDefault();
                newDefault.IsActive = true;
            }

            db.SaveChanges();

            return Json(new[] { cred }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult BUILDERCreds_Delete([DataSourceRequest]DataSourceRequest request, CatalogCredential cred)
        {
            // don't delete if this is the User's only BUILDER credentials
            var userCreds = db.CatalogCredentials.Where(w => w.AspNetUserId == cred.AspNetUserId).ToList();

            if (userCreds.Count > 1)
            {
                var entity = userCreds.Where(w => cred.Id == w.Id).FirstOrDefault();

                // if entity is selected, we need to choose another
                if ((bool)entity.IsActive)
                {
                    var newDefault = userCreds.Where(w => w.Id != cred.Id).FirstOrDefault();
                    newDefault.IsActive = true;
                }

                db.CatalogCredentials.Remove(entity);
                db.SaveChanges();

                return Json(new[] { cred }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["UserAlert"] = "Cannot delete the only active BUILDER credentials.";
                TempData["AlertType"] = "error";
                return Json( null , JsonRequestBehavior.AllowGet);
            }
        }
    }
}