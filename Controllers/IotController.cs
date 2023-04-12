using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using UiIoT.Core;
using UiIoT.Hubs;
using UiIoT.Models;

namespace UiIoT.Controllers
{
    public class IotController : Controller
    {
        private UDP _helper_udp = new UDP("127.0.0.1", 5555);
        private Command comm = new Command();
        private IoTContext ioTContext = new IoTContext();
        private readonly IHubContext<RandomDataHub> _randomdatahub;
        IoT robot1 = new IoT("s","s","s",20);
        IoT robot2 = new IoT("a", "a", "a", 20);
        private void pseudodata()
        {
            robot2.name = "гена";
            robot1.name = "Жора";
            ioTContext.robots.Add(robot1);
            ioTContext.robots.Add(robot2);
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
            _randomdatahub = randomdatahub;
            pseudodata();
        }
        public async Task<Command> udp_listener()
        {

            return await _helper_udp.ReceiveCommandAsync();
        }
       

        public async Task<IActionResult> Index()
        {    
            //TODO: take robotList
            return View(ioTContext);
        }

        public IActionResult Robot(string name)
        {
            IoT data = take_by_name(ioTContext, name);
            if (data is not null)
                return View(data);
            else
                return NotFound();
        }
       
    }
}
