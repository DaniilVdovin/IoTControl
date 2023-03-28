using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTControl.Core
{
    public class Command
    {
        public string Data;
        public Command() { }
        public Command(System.Net.Sockets.UdpReceiveResult result)
        {
            Data = Encoding.UTF8.GetString(result.Buffer);
        }
    }
}
