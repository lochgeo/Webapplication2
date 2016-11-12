using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AccountRegistry.Startup))]
namespace AccountRegistry
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
