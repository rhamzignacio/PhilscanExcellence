using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhilscanExcellence.Startup))]
namespace PhilscanExcellence
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
