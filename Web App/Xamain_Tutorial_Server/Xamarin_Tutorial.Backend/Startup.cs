using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Xamarin_Tutorial.Backend.Startup))]
namespace Xamarin_Tutorial.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
