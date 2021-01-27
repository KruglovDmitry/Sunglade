using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestFlight.Startup))]
namespace TestFlight
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
