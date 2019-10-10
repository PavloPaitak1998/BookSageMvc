using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Server_WEB_Programming.Lab2.Startup))]
namespace Server_WEB_Programming.Lab2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
