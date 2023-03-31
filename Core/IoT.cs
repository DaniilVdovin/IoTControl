using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace UiIoT.Core
{
    public  class IoT: IOfThings
    {
  

        public string type { get ; set; }
        public string name { get; set; }
        public string hostname { get; set; }
        public int port { get; set; }
        public Thread thread { get ; set; }
        public UDP UDP { get; set; }

        public IoT(string type, string name, string hostname, int port)
        {
            this.type = type;
            this.name = name;
            this.hostname = hostname;
            this.port = port;
        }
        public IoT(string[] data)
        {
            this.type = data[0];
            this.name = data[4];
            this.hostname = data[2];
            this.port = int.Parse(data[3]);
        }
        public void Start(Thread t)
        {
            if(thread != null)
                thread.Abort();
            thread = t;
            thread.Start();
        }
        public void SetupUPD() => UDP = new UDP(hostname,port);
    }
}
