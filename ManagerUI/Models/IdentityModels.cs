using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using CatalogData;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ManagerUI.Controllers;
using CatalogData.Classes;

namespace ManagerUI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        private static CMEntities db = new CMEntities();

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            // Add custom user claims here
            var userId = userIdentity.GetUserId();
            var creds = db.CatalogCredentials.Where(w => w.AspNetUserId == userId && (bool)w.IsActive).FirstOrDefault();

            // BUILDER Instance variables
            userIdentity.AddClaim(new Claim("BuilderInstanceId", creds.BUILDERInstanceId.ToString()));
            userIdentity.AddClaim(new Claim("BuilderInstance", creds.BUILDERInstance.Name));
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}