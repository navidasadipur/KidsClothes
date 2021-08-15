using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KidsClothes.Web.Startup))]
namespace KidsClothes.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
