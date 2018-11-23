using CatalogData;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerUI.Models.AspNetUsers
{
    public class CatalogUserManager : UserManager<CatalogUser>
    {
        public CatalogUserManager(IUserStore<CatalogUser> store) : base(store)
        {

        }

        public static CatalogUserManager Create(IdentityFactoryOptions<CatalogUserManager> options, IOwinContext context)
        {
            var manager = new CatalogUserManager(new UserStore<CatalogUser>(context.Get<CMEntities>()));
            return manager;
        }
    }
}