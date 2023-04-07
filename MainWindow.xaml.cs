using IoTControl.Core;
using IoTControl.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
		string[] RobotsParm = {"X", "Y", "T", "Z", "V"}; // мб должно быть и "G", но на новой прошивке "Z" + "N" т.к его нет на новых прошивках
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
                tb_log.Text = cmd.Data + "\n";
            });
        }
        
        public void SendDataToRobot(object sender, RoutedEventArgs e)
        {
            string Cmd_package = Cmd_packageZero;

            for (int i = 0;i < InputControl.Count;i++)
            {
                Cmd_package += ":" + InputControl[i].Value;//InputControl[0].Value = 10;
			}

            Cmd_package += "#";
		
            Debug.WriteLine(ThingsList.SelectedIndex);
			_ = Teams[ThingsList.SelectedIndex].UDP.SendCommandAsync(Cmd_package);
		}

	

		private void ThingsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			string[] Parameters = {};
			string Item = ThingsList.SelectedValue.ToString();
			switch (Teams[ThingsList.SelectedIndex].type)
			{
				case "P":
				case "M":
				case "M1":
				case "P1": // роботы. Отправка отличается наличием N. у букв с цифрками есть N, но на новых прошивках в этом нет смысла 
					Parameters = RobotsParm; Cmd_packageZero = "g"; break;
				case "R":
				case "R1":
				case "R2": // терминалы. Они чем-то отличаются, но нигде не сказано чем и как
					Parameters = TerminalParm; Cmd_packageZero = "r"; break;
				case "C": // камера
					Parameters = CameraParm; Cmd_packageZero = "c"; break;
				case "T": // Лампы 
					Parameters = TrafficParm; Cmd_packageZero = "l"; break; //o(*￣▽￣*)ブ　 4 и 2 перепутаны. Можно попробовать сделать foreach для него или senddatato... 
				case "D": // Диспенсер																																						 Dispenser
					Parameters = DispenserParm; Cmd_packageZero = "p"; break;
				case "B": // на него отправлять ничего не нужно																																 BarcodeReader
				case "B1": // на него отправлять ничего не нужно																											                 LightBarrier
				case "L": // сервисный логический (мобильный) робот OMG ☆*: .｡. o(≧▽≦)o .｡.:*☆        В данном контексте - OMG=ЧТОЭТОЯНЕПОНИМАЮАЛЛО
				case "AR": // дополненная реальность OMG ᓚᘏᗢ
				case "CS": // составное модульное смарт-устройство OMG (❁´◡`❁)
					break;
				default:
					break;

			}
			Container_parameters.Children.Clear(); InputControl.Clear();
			foreach (string s in Parameters)
			{
				InputControlWlegend temp = new InputControlWlegend(s);
				InputControl.Add(temp);
				Container_parameters.Children.Add(temp);
			}
			Debug.WriteLine(Teams[ThingsList.SelectedIndex].type);
		}
	}
}
