using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IoTControl.Core
{
    public class Command
    {
        public string Data;
        public Command() { }
        public Command(System.Net.Sockets.UdpReceiveResult result, IPEndPoint ipport) // System.Net.Sockets.UdpReceiveResult byte[] result
		{
            Data = ipport + " \n" + Encoding.UTF8.GetString(result.Buffer); //Encoding.UTF8.GetString(result.Buffer); Encoding.ASCII.GetString(result, 0, result.Length);
		}
    }
}
