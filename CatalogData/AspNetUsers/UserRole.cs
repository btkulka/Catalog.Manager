using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CatalogData.AspNetUsers
{
    public class UserRole : IdentityRole
    {
        public UserRole() : base()
        {

        }

        public UserRole(string name) : base(name)
        {

        }
    }
}