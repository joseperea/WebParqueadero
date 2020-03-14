using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebParqueadero.Startup))]
namespace WebParqueadero
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
