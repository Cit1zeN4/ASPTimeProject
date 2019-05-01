using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASPTime.Startup))]
namespace ASPTime
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
