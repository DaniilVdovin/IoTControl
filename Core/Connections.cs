using System.Diagnostics;
using UiIoT.Core;
using UiIoT.Models;

namespace UiIOT.Core
{
    //TODO: переписать всё навхрен!!!!!!!
    public static class Connections
    {
        public static EventHandler<Command>? NewCommand;
        public static List<IoT> Things = new List<IoT>();
        public static List<Command> CommandList = new List<Command>();

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
                            Command cmd =  i.UDP.ReceiveCommandAsync(i).Result;
                            if (cmd != null) AddCommand(i, cmd);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                            /*AddCommand(i, new Command(){ Data = e.Message });*/
						Thread.Sleep(500);
					}
					}
                }));
            }
        }
        public static void Close()
        {
            foreach (IoT i in Things)
            {
                _ = i.UDP.SendCommandAsync("s").Result;
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
