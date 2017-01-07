using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CareerVisa.Startup))]
namespace CareerVisa
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.MapSignalR();
        }
    }
}
