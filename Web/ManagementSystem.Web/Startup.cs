using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ManagementSystem.Web.Startup))]
namespace ManagementSystem.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
