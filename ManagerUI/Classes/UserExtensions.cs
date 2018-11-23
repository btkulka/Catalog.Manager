using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace ManagerUI.Classes
{
    public static class UserExtensions
    {
        public static string GetBuilderInstanceName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("BuilderInstance");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetBuilderInstanceId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("BuilderInstanceId");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}