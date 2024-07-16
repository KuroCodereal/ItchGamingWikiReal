using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ItchGamingWiki.Startup))]
namespace ItchGamingWiki
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            
        }
    }
}
