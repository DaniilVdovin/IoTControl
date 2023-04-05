using IoTControl.Core;
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
        public MainWindow()
        {
            InitializeComponent();
            Connections.NewCommand += NowNewCommand;
			Debug.WriteLine("qeweqwewqqwe");
			Connections.Things = TeamLoadManager.LoadTeams()[0].IoTs;

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
                tb_log.Text += cmd.Data + "\n";
            });
        }
        
    }
}
