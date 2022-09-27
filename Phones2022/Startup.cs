using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Phones2022.Startup))]
namespace Phones2022
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
