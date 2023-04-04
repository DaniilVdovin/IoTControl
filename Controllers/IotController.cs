using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Text;
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
        RobotViewModel robot1 = new RobotViewModel();
        RobotViewModel robot2 = new RobotViewModel();

        public IotController(IHubContext<RandomDataHub> randomdatahub)
        {
            _randomdatahub = randomdatahub;
        }
        public async Task<Command> udp_listener()
        {

            return await _helper_udp.ReceiveCommandAsync();
        }

        public async Task<IActionResult> Index()
        {
            robot2.name = "гена";
            //Command data = await udp_listener();
            //Debug.WriteLine( data.Data + robot2.name);
         
                
               


            

            robot1.name = "Жора";
            ioTContext.robots.Add(robot1);
            ioTContext.robots.Add(robot2);
            return View(ioTContext);
        }

        public IActionResult Robot(int id, string type)
        {
            ioTContext.robots.Add(robot1);
            ioTContext.robots.Add(robot2);
            return View("robot", ioTContext.robots[0]);
        }
    }
}
