using Owin;

namespace UiIoT
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {

        }

        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}
