using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LaunchKey.Web.Startup))]
namespace LaunchKey.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
