using Microsoft.AspNetCore.Mvc;
using UiIoT.Core;
using UiIoT.Models;

namespace UiIoT.Controllers
{
    public class IotController : Controller
    {
        private UDP _helper_udp = new UDP("127.0.0.1",5555);
        private Command comm = new Command();
        private IoTContext  ioTContext = new IoTContext();
        RobotViewModel robot1 = new RobotViewModel();
        RobotViewModel robot2 = new RobotViewModel();
        
        public IActionResult Index()
        {
            robot2.name = "гена";

            robot1.name = "Жора";
            ioTContext.robots.Add(robot1);
            ioTContext.robots.Add(robot2);
            return View(ioTContext);
        }
        
        public IActionResult Robot(int id,string type)
        {

            return View("robot", ioTContext.robots);
        }
    }
}
