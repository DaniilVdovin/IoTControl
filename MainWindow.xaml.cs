using IoTControl.Core;
using System;
using System.Collections.Generic;
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

            Connections.Things.Add(new IoT("M", "Robot 1", "127.0.0.1", 5555));
            Connections.Things.Add(new IoT("M", "Robot 2", "127.0.0.1", 5555));
            Connections.Things.Add(new IoT("M", "Robot 3", "127.0.0.1", 5555));
            Connections.Things.Add(new IoT("M", "Robot 4", "127.0.0.1", 5555));
            Connections.Things.Add(new IoT("M", "Robot 5", "127.0.0.1", 5555));

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
                tb_log.Text += ((IoT)s).name +  " : " + cmd.Data + "\n";
            });
        }
        
    }
}
