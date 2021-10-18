using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Zadanie_Rekrutacyjne.Startup))]
namespace Zadanie_Rekrutacyjne
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
