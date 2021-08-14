using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(drugStore7.Web.Startup))]
namespace drugStore7.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
