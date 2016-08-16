using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EventBot.Web.Startup))]
namespace EventBot.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
