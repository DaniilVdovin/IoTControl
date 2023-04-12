using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IoTControl.Core
{
    public class IoT
    {
        public string type;
		public string firstLetter; // первая буква в пакете управления
		public string name;
        public string hostname; 
        public int port;
		public string service;
		public Thread thread;
        public UDP UDP;
		Dictionary<string, string> ThingMonitoring = new Dictionary<string, string>();
		Dictionary<string, string> ThingControl = new Dictionary<string, string>();

		// надо добавить переменную которая будет хранить пришедшие ей данные, тогда будет меньше проблем ToDictionary

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
            this.service = data[5];

        }
       
        public void Start(Thread t)
        {
            if(thread != null)
                thread.Abort();
            thread = t;
            thread.Start();
        }
        public void SetupUPD() => UDP = new UDP(hostname,port);

        private void TemplateFill(string type)
		{
			

		}

	}
}
