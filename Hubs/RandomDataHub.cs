using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using UiIoT.Models;

namespace UiIoT.Hubs
{
    public class RandomDataHub : Hub
    {

        public async Task Send(string data)
        {

            await Clients.Caller.SendAsync("Receive", "ss");
        }

    }

}
