using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace IoTControl.Core
{
    public static class Connections
    {
        public static EventHandler<Command> NewCommand;
        public static List<IoT> Things = new List<IoT>();
        public static List<Command> CommandList = new List<Command>();

        public static void Start()
        {
            foreach (IoT i in Things)
            {
                i.SetupUPD();
                i.Start(new Thread(async () =>
                {

                    bool work = true;
                    _ = i.UDP.SendCommandAsync("#N").Result;
                    while (work)
                    {
                        try
                        {
                            Command cmd = i.UDP.ReceiveCommandAsync().Result;
                            if (cmd != null) AddCommand(i, cmd);
                        }
                        catch (Exception)
                        {
                            AddCommand(i, new Command(){ Data = "UDP ERROR" });
                        }
                        Thread.Sleep(1000);
                    }
                }));
            }
        }
        public static void Close()
        {
            foreach (IoT i in Things)
            {
                _ = i.UDP.SendCommandAsync("#C").Result;
                i.thread.Abort();
                i.UDP.Close();
            }
        }
        internal static void AddCommand(IoT ioT, Command cmd)
        {
            CommandList.Add(cmd);
            NewCommand.Invoke(ioT, cmd);
        }
    }
}
