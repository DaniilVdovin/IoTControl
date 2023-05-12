using IoTControl.Core;
using IoTControl.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Packaging;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace IoTControl
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

		List<string> ListThingsName = new List<string>();
		List<string> ListBarcodeThingsName = new List<string>();
		List<IoT> ListBarcode = new List<IoT>();

		List<Team> Teams = new List<Team>();
		List<InputControlWlegend> InputControl = new List<InputControlWlegend>();
		public static DataForThingworx DFThx = new DataForThingworx();
		public MainWindow()
        {
			InitializeComponent();
            Connections.NewCommand += NowNewCommand;
            Teams = TeamLoadManager.LoadTeams();
			//DataForThingworx.LoadThx();

			Connections.Things = Teams[0].IoTs;
			Connections.DFThx = DFThx;
			Connections.Start();
			List<string> ListForTeams = new List<string>(); 
            foreach (Team team in Teams)
            {
				TeamsList.Items.Add(team.Name);
			}
			//TeamsList_SelectionChanged(null, null);
			


			tb_appKey.Text = DataForThingworx.AppKey;
			tb_serverIP.Text = DataForThingworx.ServerIP;

            Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
        }

        private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            Connections.Close();
            Connections.NewCommand -= NowNewCommand;
        }
		private void TeamsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (Connections.Things != null) { Connections.Close(); }
			Connections.Things = Teams[TeamsList.SelectedIndex].IoTs;
			DataForThingworx.ServerIP = Teams[TeamsList.SelectedIndex].ServerIP;
			DataForThingworx.AppKey = Teams[TeamsList.SelectedIndex].Appkey;
			Connections.Start();

			ListBarcode.Clear(); ListThingsName.Clear(); ListBarcodeThingsName.Clear(); 
			BarcodeList.Items.Clear(); ThingsList.Items.Clear(); 

			foreach (IoT t in Teams[TeamsList.SelectedIndex].IoTs)
			{
				if (t.type == "B")
				{
					ListBarcode.Add(t);
					BarcodeList.Items.Add(t.name);
				}
				ThingsList.Items.Add(t.name);

			}
			ThingsList.SelectedIndex = 0; ThingsList.SelectedItem = 0;
			BarcodeList.SelectedIndex = 0; BarcodeList.SelectedItem = 0;
			

		}
		public void NowNewCommand(object s,Command cmd)
        {
            Dispatcher.Invoke(() =>
            {
				GetReceiveFromThingworx(cmd.Response, cmd.ThingSelf);
				var valzxc = "";
				if (cmd.Data.Length < 4) cmd.Data = null;
				

				if (cmd.Response == null) { }
				else
				{
					foreach (var value in cmd.Response)
					{
						valzxc += value.ToString();
					}
					valzxc = "\n" + valzxc;
				}
				string textToLog = (cmd.ThingSelf.name +" "+  cmd.Data + valzxc + "\n");
				string textToMonitor = (cmd.Data != null ? cmd.ThingSelf.name + " " + cmd.Data + "\n" : "");
				Debug.WriteLine(tb_log.Text.Length);
				if (tb_log.Text.Length > 10000)
					tb_log.Text = tb_log.Text.Substring(tb_log.Text.Length-textToLog.Length, textToLog.Length); // надо пофиксить 

				if (tb_Monitoring.Text.Length > 10000)
					tb_Monitoring.Text = tb_Monitoring.Text.Substring(tb_Monitoring.Text.Length - textToMonitor.Length, textToMonitor.Length); // надо пофиксить 

				tb_Monitoring.Text += textToMonitor;
				tb_log.Text += textToLog;
			});
        }

		public void SendDataToRobot(object sender, RoutedEventArgs e)
		{
			for (int i = 0; i < InputControl.Count; i++)
			{
				Connections.Things[ThingsList.SelectedIndex].ThingControl[InputControl[i].getLegend()] = InputControl[i].Value;
			}
			SendData(InputControl.Count, Connections.Things[ThingsList.SelectedIndex]);
		}
		public void SendDataToRobot(int Param, IoT ThingSelf)
		{
			SendData(Param,ThingSelf);
		}
		private void SendData(int Param, IoT ThingSelf)
		{
			string Cmd_package = ThingSelf.firstLetter;

			for (int i = 0; i < Param; i++)
			{
				Cmd_package += ":" + ThingSelf.ThingControl.ElementAt(i).Value;
			}

			Cmd_package += "#";

			Debug.WriteLine(ThingsList.SelectedIndex);
			_ = ThingSelf.UDP.SendCommandAsync(Cmd_package);
		}
		

		private void ThingsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ThingsList.SelectedIndex != -1)
				ChangeKeyValue(Connections.Things[ThingsList.SelectedIndex]);
			else
				ChangeKeyValue(Connections.Things[0]);
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
		}
		private void GetReceiveFromThingworx(Dictionary<string,string> Responce, IoT ThingSelf) 
		{
			if (ToggleReceiveFromThingworx.IsChecked == true)
			{
				foreach (var item in ThingSelf.ThingControl.Keys.ToList())
				{
					if (Responce.ContainsKey(item))
					{
						ThingSelf.ThingControl[item] = Responce[item];
						if (Connections.Things[ThingsList.SelectedIndex] == ThingSelf)
							ChangeKeyValue(ThingSelf);
					}
				}
				SendDataToRobot(ThingSelf.ThingControl.Count, ThingSelf);

			}
		}
		public string GetTextValue(TextBox txtboxName)
		{
			return txtboxName.Text;
		}

		private void SendForAll(object sender, RoutedEventArgs e)
		{
			Connections.SendForAllThings(((Button)sender).Tag.ToString());
		}

		private void SaveDataForConnectToThingworx(object sender, RoutedEventArgs e)
		{
			DataForThingworx.ServerIP = tb_serverIP.Text;
			DataForThingworx.AppKey = tb_appKey.Text;
			DataForThingworx.SaveThx(tb_appKey.Text, tb_serverIP.Text, Teams[TeamsList.SelectedIndex].Path);
		}

		private async void SendNumberForThing(object sender, RoutedEventArgs e)
		{
			await Connections.Things[ThingsList.SelectedIndex].UDP.SendCommandAsync(((Button)sender).Tag.ToString());
		}
		private void SendCodeToThingworx(object sender, RoutedEventArgs e)
		{
			if (BarcodeList.SelectedIndex != -1)
				_  = Thingworx.SendToThingworx(ListBarcode[(BarcodeList.SelectedIndex)], new Dictionary<string, string> {{ "c", textBox_Barcode.Text } });
		}


		private void Monitoring_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			scroll_Monitoring.ScrollToBottom();
		}

		private void Log_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			scroll_log.ScrollToBottom();
		}

		private void ToggleReceiveToThingworx_Click(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine(ToggleReceiveToThingworx.IsChecked);
			Thingworx.SendToThx = ToggleReceiveToThingworx.IsChecked ?? false;
		}

		private void ToggleReceiveFromThingworx_Click(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine(ToggleReceiveFromThingworx.IsChecked);
			Thingworx.ReceiveFromThx = ToggleReceiveFromThingworx.IsChecked ?? false;

		}
	}
}
