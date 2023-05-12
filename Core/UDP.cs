using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows.Markup;
using System.Net;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace IoTControl.Core
{
	public class UDP
    {
		UdpClient udpClient;
		IPEndPoint groupEP;
		public UDP(string hostname, int port)
        {
			udpClient = new UdpClient(hostname, port);
			groupEP = new IPEndPoint(IPAddress.Parse(hostname), 8888);
		}
		public void Reconect(string hostname, int port)
        {
            udpClient.Close();
            udpClient.Connect(hostname, port);
        }
        public void Close() => udpClient.Close();
		public async Task<Command> ReceiveCommandAsync(IoT i)
		{
			try
			{
				UdpClient listener = new UdpClient(i.port);
				var response = await listener.ReceiveAsync();
				Debug.WriteLine(response.RemoteEndPoint.Address.ToString());
				if (response.RemoteEndPoint.Address.ToString() == i.hostname)
				{
					Debug.WriteLine(response.Buffer);
					listener.Close();
					var temp = Thingworx.Connect(response, i);
					return (new Command(response.Buffer, groupEP, temp.Result, i));
				}
				listener.Close();
				return null;
			}
			catch {
				return null;
			}

		}
        public async Task<bool> SendCommandAsync(string command)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(command);
                await udpClient.SendAsync(data, data.Length);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
