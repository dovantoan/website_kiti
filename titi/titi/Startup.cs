using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(titi.Startup))]
namespace titi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
