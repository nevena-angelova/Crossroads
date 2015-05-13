using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Crossroads.Web.Startup))]
namespace Crossroads.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
