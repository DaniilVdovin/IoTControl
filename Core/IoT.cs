﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IoTControl.Core
{
    public class IoT
    {
        public string type;
        public string name;
        public string hostname; 
        public int port;
        public Thread thread;
        public UDP UDP;

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
