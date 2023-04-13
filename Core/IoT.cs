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

		/*public delegate Dictionary<string, string> DelegateForThings();
		public DelegateForThings GetDataForThings;*/ // можно будет попробовать такой вариант

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
			TemplateFill(this.type);

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
			switch (type)
			{
				case "P":
				case "P1": //роботы. Отличаются наличием N, в роботах с 1 он есть (сейчас это не актуально, потому что новая прошивка сделала всех роботов типом без 1)
					firstLetter = "p"; ThingMonitoring = GetRobotsData(("[M:0:0:0:0:0:0:0:0:0#T:0:0:0:0:0:0:0:0:0#L:0:0:0:0:0:0:0:0:0#]").Split('#')); ThingControl = new Dictionary<string, string>() { { "X","0" }, { "Y","0" }, { "Z","0" }, { "V", "0" } };break;
				case "M":
				case "M1":
					firstLetter = "g"; ThingMonitoring = GetRobotsData(("[M:0:0:0:0:0:0:0#T:0:0:0:0:0:0:0#L:0:0:0:0:0:0:0#]").Split('#')); ThingControl = new Dictionary<string, string>() { { "X", "0" }, { "Y", "0" }, { "T", "0" }, { "Z", "0" }, { "V", "0" } }; break;
				case "R":
				case "R1":
				case "R2": //терминалы. Они чем-то отличаются, но нигде не сказано чем и как
					firstLetter = "r"; ThingMonitoring = GetTerminalData(("[?:0:0:0:0#]").Split('#')); ThingControl = new Dictionary<string, string>() { { "L1", "0" }, { "L2", "0" }, { "L3", "0" }, { "L4", "0" }, { "D1", "0" }, { "DT", "0" } }; break;
				case "C": //камера
					firstLetter = "c"; ThingMonitoring = GetCameraData(("[?:00000:00000#]").Split('#')); ThingControl = new Dictionary<string, string>() { { "G", "0" } }; break;
				case "T": // не получает данные с устройства  TrafficLight
					firstLetter = "l"; ThingControl = new Dictionary<string, string>() { { "L1", "0" }, { "L2", "0" }, { "L3", "0" }, { "L4", "0" }}; break;
				case "D": // то же самое(B1), но [D:"n":"s":"c"#] (не уверен), где n - lastcommandnumber, c - count, s - status.																 Dispenser
				case "B": // нужно делать самостоятельно(т.к такой вещи не существует, с оригинального IoT control center приходит огромный, бесполезный пакет данных). отправлять нужно "c" BarcodeReader
				case "B1": // такого у нас нет, исходя из логики, можно просто предположить, что там приходит примерно такой пакет [B:"d1":"d2":"d3"#], где ключи это значения               LightBarrier
				case "L": // сервисный логический (мобильный) робот OMG ☆*: .｡. o(≧▽≦)o .｡.:*☆        В данном контексте - OMG=ЧТОЭТОЯНЕПОНИМАЮАЛЛО
				case "AR": // дополненная реальность OMG ᓚᘏᗢ
				case "CS": // составное модульное смарт-устройство OMG (❁´◡`❁)
					firstLetter = ""; ThingMonitoring = null; ThingControl = new Dictionary<string, string>() { }; break;
				default:
					firstLetter = ""; ThingMonitoring = null; ThingControl = new Dictionary<string, string>() { }; break;
			}

		}
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

			Data.Add("c", Subs[1].Split(':')[2]); // видимо, с последним патчем прошивки робота он перестал приходить(или никогда не приходил (_　_)。゜zｚＺ )
			Data.Add("s", Subs[1].Split(':')[2]);
			Data.Add("n", Subs[2].Split(':')[3]);

			this.ThingMonitoring = Data;
			return Data;
		}
		public Dictionary<string, string> GetTerminalData(string[] Subs)
		{
			var Data = new Dictionary<string, string>();
			Data.Add("p", Subs[0].Split(':')[1]);
			Data.Add("b1", Subs[0].Split(':')[2]);
			Data.Add("b2", Subs[0].Split(':')[3]);
			Data.Add("b3", Subs[0].Split(':')[4]);

			this.ThingMonitoring = Data;
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
			
			this.ThingMonitoring = Data;
			return Data;
		}

	}
}
