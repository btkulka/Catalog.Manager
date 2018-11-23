using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagerUI.Models;

namespace ManagerUI.Controllers
{
    public class UserController : CMController
    {

        public ActionResult Index(string userId)
        {
            // Fetch user for view model
            var user = db.AspNetUsers.Where(w => w.Id == userId).FirstOrDefault();
            AspNetUserAccountViewModel viewModel = new AspNetUserAccountViewModel();
            viewModel.User = user;

            return View(viewModel);
        }
    }
}