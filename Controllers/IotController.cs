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
        public IActionResult Index()
        {
            RobotViewModel robot1 = new RobotViewModel();
            robot1.name = "Жора";
            RobotViewModel robot2 = new RobotViewModel();
            robot2.name = "гена";

            ioTContext.robots.Add(robot1);
            ioTContext.robots.Add(robot2);
            return View(ioTContext);
        }
        public string test()
        {
            return "RobotGraphfics here";
        }
    }
}
