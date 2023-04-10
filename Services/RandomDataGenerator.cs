using Microsoft.AspNetCore.SignalR;
using UiIoT.Hubs;


namespace UiIoT.Services
{
    public class RandomDataGenerator : BackgroundService
    {
        private readonly IHubContext<RandomDataHub> _hub;
        private readonly Buffer<Point> _data;


        public RandomDataGenerator(IHubContext<RandomDataHub> hub, Buffer<Point> data)
        {
            _hub = hub;
            _data = data;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            while (!stoppingToken.IsCancellationRequested)
            {
                await _hub.Clients.All.SendAsync(
                    "newData",
                    _data.AddNewRandomPoint(),
                    cancellationToken: stoppingToken
                );
               

                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);

            }
        }
    }
}
