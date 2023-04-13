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
				GetReceiveFromThingworx(cmd.Response, cmd.ThingSelf);
				tb_log.Text = cmd.Data + "\n" + cmd.Response.ToArray() + "I HATE EVERYTHING";
			});
        }

		public void SendDataToRobot(object sender, RoutedEventArgs e)
		{
			SendData(InputControl.Count, Teams[ThingsList.SelectedIndex]);
		}
		public void SendDataToRobot(int Param, IoT ThingSelf)
		{
			SendData(Param,ThingSelf);
		}
		private void SendData(int Param, IoT ThingSelf) // мб нужно сделать async 
		{
			string Cmd_package = ThingSelf.firstLetter;

			for (int i = 0; i < Param; i++)
			{
				ThingSelf.ThingControl[InputControl[i].legend] = InputControl[i].Value;
				Cmd_package += ":" + ThingSelf.ThingControl[InputControl[i].legend];
			}

			Cmd_package += "#";

			Debug.WriteLine(ThingsList.SelectedIndex);
			_ = ThingSelf.UDP.SendCommandAsync(Cmd_package);
		}
		

		private void ThingsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ChangeKeyValue(Teams[ThingsList.SelectedIndex]);
		}
		public void ChangeKeyValue(IoT things)
		{
			Container_parameters.Children.Clear(); InputControl.Clear();
			foreach (string s in things.ThingControl.Keys)
			{
				InputControlWlegend temp = new InputControlWlegend(s, things.ThingControl[s].ToString());
				InputControl.Add(temp);
				Container_parameters.Children.Add(temp);
			}
			Debug.WriteLine(Teams[ThingsList.SelectedIndex].type);
		}
		private void GetReceiveFromThingworx(Dictionary<string,string> Responce, IoT ThingSelf) 
		{
			
			if (ToggleReceive.IsChecked == true)
			{
				foreach (var item in ThingSelf.ThingControl.Keys.ToList())
				{
					if (Responce.ContainsKey(item))
					{
						ThingSelf.ThingControl[item] = Responce[item];
						//if (Teams[ThingsList.SelectedIndex] == ThingSelf)
						ChangeKeyValue(ThingSelf);  //TODO будет перезаписывать данные на экране (  Но нужно же делать красиво а не так чтобы оно работало да? (❁´◡`❁)(❁´◡`❁)(❁´◡`❁) )
					}
				}
				SendDataToRobot(ThingSelf.ThingControl.Count, ThingSelf);

			}

		}
	}
}
