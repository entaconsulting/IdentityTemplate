using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IdentityManagerWebApp.Startup))]
namespace IdentityManagerWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            AutomapperConfiguration.Configure();
        }
    }
}
