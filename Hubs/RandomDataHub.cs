using Microsoft.AspNetCore.SignalR;

namespace UiIoT.Hubs
{
    public class RandomDataHub : Hub
    {
        public static string url = "robots";
        public async Task Send(string data)
        {

            await Clients.Caller.SendAsync("Receive", "ss");
        }

    }

}
