using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace IoTControl.Core
{
    public static class Connections
    {
        public static EventHandler<Command> NewCommand;
        public static List<IoT> Things = new List<IoT>();
        public static List<Command> CommandList = new List<Command>();
		public static DataForThingworx DFThx;

		public static void Preload()
        {

        }
		public static void Start()
		{
			foreach (IoT i in Things)
			{
				i.SetupUPD();
				i.Start(new Thread(async () =>
				{
					bool work = true;
					await i.UDP.SendCommandAsync("r");
					while (work)
					{
						try
						{
							Command cmd = new Command();
							if (Thingworx.ReceiveFromThx)
							{
								var temp = Thingworx.ReceiveFromThingworx(i);
								cmd = new Command(new byte[0], null, temp.Result, i);
								if (cmd != null) AddCommand(i, cmd);
							}
							cmd = i.UDP.ReceiveCommandAsync(i).Result;
							if (cmd != null) AddCommand(i, cmd);
						}
						catch (Exception e)
						{
							Debug.WriteLine(e.Message + " connections catch\n" + e.StackTrace);
						}
						//await Task.Delay(500);
						Thread.Sleep(2000); //FIXME Нужно что-то с этим делать, беспощадно лагать может
					}
				}));
			}
		}
		public static async void Close()
        {
            foreach (IoT i in Things)
            {
                await i.UDP.SendCommandAsync("s");
                i.thread.Abort();
                i.UDP.Close();
            }
        }
        internal static void AddCommand(IoT ioT, Command cmd)
        {
            CommandList.Add(cmd); // нужен ли вообще?
            NewCommand.Invoke(ioT, cmd);
        }
		public static async void SendForAllThings(string letter)
		{
            foreach (IoT i in Things)
            {
                await i.UDP.SendCommandAsync(letter);
            }
		}
	}
}

