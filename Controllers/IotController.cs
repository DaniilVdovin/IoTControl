using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using UiIoT.Core;
using UiIoT.Hubs;
using UiIoT.Models;

namespace UiIoT.Controllers
{
    public class IotController : Controller
    {
        private readonly UDP _helper_udp = new UDP("127.0.0.1", 5555);
        private readonly IoTContext ioTContext = new IoTContext();
        private void pseudodata()
        {
            IoTParser parser = new IoTParser();
            string data = pseudoDataWorker.take_pseudodate();

            for (int i = 0; i < 3; i++)
            {
                ioTContext.robots.Add(parser.Parse(data));
            }
        }
        private IoT take_by_name(IoTContext ioTContext, string name)
        {
            foreach (IoT robot in ioTContext.robots)
            {
                if (robot.name == name)
                {
                    return robot;
                }
            }
            return null;

        }
        
        public IotController(IHubContext<RandomDataHub> randomdatahub)
        {
           

        }
        public async Task<Command> udp_listener()
        {

            return await _helper_udp.ReceiveCommandAsync();
        }
       

        public async Task<IActionResult> Index()
        {
            pseudodata();
            //TODO: take robotList
            return View(ioTContext);
        }

        public IActionResult Robot(string name)
        {

            udp_listener();
            IoT data = take_by_name(ioTContext, name);
            if (data is not null)
                return View(data);
            else
                return NotFound();
        }
       
    }
}
