using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ManagerUI.Startup))]
namespace ManagerUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
