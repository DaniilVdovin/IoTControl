using UiIoT.Core;

namespace UiIoT.Models
{
    public class RobotViewModel : IOfThings
    {
        public string type { get; set; }
        public string name { get; set; }
        public string hostname { get; set; }
        public int port { get; set; }
        public Thread thread { get; set; }
        public UDP UDP { get; set; }

        public RobotViewModel() => this.type = "robot";





    }
}
