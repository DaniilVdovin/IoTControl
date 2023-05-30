using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;

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
		public Dictionary<string, string> ThingMonitoring = new Dictionary<string, string>();
		public Dictionary<string, string> ThingControl = new Dictionary<string, string>();


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
			this.ThingMonitoring = new ThingProperties().GetValue(type, "monitoring");
			this.ThingControl = new ThingProperties().GetValue(type, "control");
			this.firstLetter = new ThingProperties().getFirstLetter(type);

		}
       
        public void Start(Thread t)
        {
            if(thread != null)
                thread.Abort();
            thread = t;
            thread.Start();
        }
        public void SetupUPD() => UDP = new UDP(hostname,port);
		public Dictionary<string, string> GetRobotsData(string[] Subs)
		{
			var Data = new Dictionary<string, string>();

			Debug.WriteLine(Subs[0].Length);
			for (int i = 0; i < Subs.Length - 1; i++)
			{
				var sub = Subs[i].Split(':');
				for (int j = 4; j <= 9; j++)
				{
					try
					{
						Data.Add(sub[0].ToLower() + (j - 3), sub[j]);

					}
					catch
					{
						Data.Add(sub[0].ToLower() + (j - 3), "0");

					}
				}
			}

			Data.Add("c", Subs[1].Split(':')[1]); // видимо, с последним патчем прошивки робота он перестал приходить(или никогда не приходил (_　_)。゜zｚＺ )
			Data.Add("s", Subs[1].Split(':')[2]);
			Data.Add("n", Subs[2].Split(':')[3]);

			//this.ThingMonitoring = Data;
			return Data;
		}
		public Dictionary<string, string> GetTerminalData(string[] Subs)
		{
			var Data = new Dictionary<string, string>();
			Data.Add("p", Subs[0].Split(':')[2]);
			Data.Add("b1", Subs[0].Split(':')[3]);
			Data.Add("b2", Subs[0].Split(':')[4]);
			Data.Add("b3", Subs[0].Split(':')[5]);

			//this.ThingMonitoring = Data;
			return Data;
		}
		public Dictionary<string, string> GetCameraData(string[] Subs)
		{
			var Data = new Dictionary<string, string>();

			string[] strokes = Subs[0].Split(':');
			for (int i = 1; i < strokes.Length; i++)
			{
				Data.Add(("l" + (i - 1)), strokes[i]);
			}
			
			//this.ThingMonitoring = Data;
			return Data;
		}

	}
}
