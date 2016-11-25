using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BatteryMotorMonitor.Startup))]
namespace BatteryMotorMonitor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
