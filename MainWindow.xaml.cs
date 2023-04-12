using IoTControl.Core;
using IoTControl.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IoTControl
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		string[] RobotsParmM = {"X", "Y", "T", "Z", "V"}; // мб должно быть и "G", но на новой прошивке "Z" + "N" т.к его нет на новых прошивках
		string[] RobotsParmP = { "X", "Y", "Z", "V" };

		string[] TrafficParm = { "L1", "L4" , "L3", "L2" };
		string[] TerminalParm = { "L1", "L2", "L3", "L4", "D1", "DT"};
		string[] CameraParm = { "G" };
		string[] DispenserParm = { "N", "D1" };

		string Cmd_packageZero = "?";
		List<string> ListThingsName = new List<string>();
		List<IoT> Teams = new List<IoT>();
		List<InputControlWlegend> InputControl = new List<InputControlWlegend>();
		public MainWindow()
        {
			InitializeComponent();
            Connections.NewCommand += NowNewCommand;
            Teams = TeamLoadManager.LoadTeams()[0].IoTs;
			Connections.Things = Teams;
            foreach (IoT t in Teams)
            {
				ListThingsName.Add(t.name);
			}

			ThingsList.ItemsSource = ListThingsName;
            Connections.Start();

            Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
        }

        private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            Connections.Close();
            Connections.NewCommand -= NowNewCommand;
        }

        public void NowNewCommand(object s,Command cmd)
        {
            Dispatcher.Invoke(() =>
            {
				GetReceiveFromThingworx(cmd.Response, cmd.ThingSelf.type, cmd.ThingSelf);
				tb_log.Text = cmd.Data + "\n" + cmd.Response;
            });
        }
        
        public void SendDataToRobot(object sender, RoutedEventArgs e)
        {
			SendData(InputControl.Count, Cmd_packageZero, Teams[ThingsList.SelectedIndex]);
		
		}
		public void SendDataToRobot(string[] Param, string pkgzero, IoT ThingSelf)
		{
			SendData(Param.Length,pkgzero,ThingSelf);
		}
		private void SendData(int Param, string pkgzero, IoT ThingSelf)
		{
			string Cmd_package = pkgzero;

			for (int i = 0; i < Param; i++)
			{
				Cmd_package += ":" + InputControl[i].Value;
			}

			Cmd_package += "#";

			Debug.WriteLine(ThingsList.SelectedIndex);
			_ = ThingSelf.UDP.SendCommandAsync(Cmd_package);
		}
		private (string[], string) ThingsProverka(string type)
		{
			string[] Param = null;
			string cmd_pkgZero = null;

			switch (type)
			{
				case "P":
				case "P1": // роботы. Отправка отличается наличием N. у букв с цифрками есть N, но на новых прошивках в этом нет смысла 
					Param = RobotsParmP; cmd_pkgZero = "p"; break;
				case "M":
				case "M1":
					Param = RobotsParmM; cmd_pkgZero = "g"; break;
				case "R":
				case "R1":
				case "R2": // терминалы. Они чем-то отличаются, но нигде не сказано чем и как
					Param = TerminalParm; cmd_pkgZero = "r"; break;
				case "C": // камера
					Param = CameraParm; cmd_pkgZero = "c"; break;
				case "T": // Лампы 
					Param = TrafficParm; cmd_pkgZero = "l"; break; //o(*￣▽￣*)ブ　 4 и 2 перепутаны. Можно попробовать сделать foreach для него или senddatato... 
				case "D": // Диспенсер																																						 Dispenser
					Param = DispenserParm; cmd_pkgZero = "p"; break;
				case "B": // на него отправлять ничего не нужно																																 BarcodeReader
				case "B1": // на него отправлять ничего не нужно																											                 LightBarrier
				case "L": // сервисный логический (мобильный) робот OMG ☆*: .｡. o(≧▽≦)o .｡.:*☆        В данном контексте - OMG=ЧТОЭТОЯНЕПОНИМАЮАЛЛО
				case "AR": // дополненная реальность OMG ᓚᘏᗢ
				case "CS": // составное модульное смарт-устройство OMG (❁´◡`❁) 
					break;
				default:
					break;
					
			}
			return (Param, cmd_pkgZero);
		}

		private void ThingsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var thingsproverka = ThingsProverka(Teams[ThingsList.SelectedIndex].type);
			Cmd_packageZero = thingsproverka.Item2;
			Container_parameters.Children.Clear(); InputControl.Clear();
			foreach (string s in thingsproverka.Item1)
			{
				InputControlWlegend temp = new InputControlWlegend(s);
				InputControl.Add(temp);
				Container_parameters.Children.Add(temp);
			}
			Debug.WriteLine(Teams[ThingsList.SelectedIndex].type);
		}
		private void GetReceiveFromThingworx(JsonObject Responce, string TypeThing, IoT ThingSelf) 
		{
			if (ToggleReceive.IsChecked == true)
			{
				var thingsproverka = ThingsProverka(TypeThing);

				for (int i = 0; i < thingsproverka.Item1.Length; i++) //TODO Fix for
				{
					for (int p=0; p < thingsproverka.Item1.Length; p++)
					{
						if (Responce.ToArray()[i].Key.ToString() == thingsproverka.Item1[p]) // нужно условие та ли вещь иначе он получает с первого робота и отправляет на второго 
						{
							try
							{
								InputControl[p].Value = Responce.ToArray()[i].Value.ToString();
							}
							catch
							{
								break;
							}
						};

					}
				}
				//SendDataToRobot(thingsproverka.Item1, thingsproverka.Item2, ThingSelf);
			}
			else { }
		}
	}
}
