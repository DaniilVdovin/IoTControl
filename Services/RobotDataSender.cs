using Microsoft.AspNetCore.SignalR;
using UiIoT.Hubs;
using UiIoT.Models;

namespace UiIoT.Services
{
    public class RobotDataSender : BackgroundService
    {
        private readonly IHubContext<RandomDataHub> _hub;

        private IoTContext ioTContext;
            
            
        //суета такая это заглущка сюда данные с сервера
        private RobotViewModel ViewModel = new RobotViewModel();
        public RobotDataSender()
        {
            ioTContext = new IoTContext();
            ioTContext.robots.Add(ViewModel);
        }
  
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                await _hub.Clients.All.SendAsync("das_send_context",this.ioTContext);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
