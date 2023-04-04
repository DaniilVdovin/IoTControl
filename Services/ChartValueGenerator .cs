﻿using Microsoft.AspNetCore.SignalR;
using UiIoT.Controllers;
using UiIoT.Hubs;
using UiIoT.Models;
using static UiIoT.Models.BufferExtensions;

namespace UiIoT.Services
{
    public class ChartValueGenerator : BackgroundService
    {
        private readonly IHubContext<RandomDataHub> _hub;
        private readonly Buffer<Point> _data;

        public ChartValueGenerator(IHubContext<RandomDataHub> hub, Buffer<Point> data)
        {
            _hub = hub;
            _data = data;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _hub.Clients.All.SendAsync(
                    "addChartData",
                    _data.AddNewRandomPoint(),
                    cancellationToken: stoppingToken
                );

                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }
        }
    }
}
