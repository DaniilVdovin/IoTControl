using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UiIoT.Core;

namespace UiIoT.Models
{
    public class IoT : IOfThings
    {

        public string type { get; set; }
        public string name { get; set; }
        public string hostname { get; set; }
        public int port { get; set; }

        public string service { get; set; }

        public Thread thread { get; set; }
        public UDP UDP { get; set; }


        public IoT(string types, string names, string hostnames, int ports)
        {
            this.type = types;
            this.name = names;
            this.hostname = hostnames;
            this.port = ports;
        }
        public IoT(string[] data)
        {
            this.type = data[0];
            this.name = data[4];
            this.hostname = data[2];
            this.port = int.Parse(data[3]);
            this.service = data[5];

        }

        public void Start(Thread t)
        {
            if (thread != null)
                thread.Abort();
            thread = t;
            thread.Start();
        }
        public void SetupUPD() => UDP = new UDP(hostname, port);
    }
}
