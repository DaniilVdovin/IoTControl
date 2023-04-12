using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace IoTControl.Core
{
    public class Command
    {
        public string Data;
		public string TypeThing;
		public JsonObject Response;
		public IoT ThingSelf;

		public Command() { }
        public Command(System.Net.Sockets.UdpReceiveResult result, IPEndPoint ipport, Task<JsonObject> Resp, IoT i) // System.Net.Sockets.UdpReceiveResult byte[] result
		{
			ThingSelf = i;
			TypeThing = i.type;
			Response = Resp.Result;
			Data = ipport + " \n" + Encoding.UTF8.GetString(result.Buffer); //Encoding.UTF8.GetString(result.Buffer); Encoding.ASCII.GetString(result, 0, result.Length);
		}
    }
}
