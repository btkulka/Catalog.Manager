using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CatalogBusiness;
using CatalogData;
using CatalogData.Classes;
using Microsoft.AspNet.Identity;

namespace ManagerUI.Controllers
{
    public class CMController : Controller
    {
        private static CatalogCredentials Creds;

        // Static Creds for Testing Purposes
        public static CatalogCredentials cred {
            get {
                return Creds;
            }
            set {
                Creds = value;
            }
        }

        public static CMEntities db = new CMEntities();

        public CatalogCredentials FetchCredentials()
        {
            if (cred == null)
            {
                var userId = User.Identity.GetUserId();
                var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();
                cred = new CatalogCredentials(creds.BUILDERInstance.RemoteServiceAddress, creds.BuilderUn, creds.BuilderPw);
            }

            return cred;
        }
    }
}