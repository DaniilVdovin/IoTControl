﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows.Markup;
using System.Net;

namespace IoTControl.Core
{
    public class UDP
    {
        UdpClient udpClient;
        public UDP(string hostname, int port)
        {
            udpClient = new UdpClient(hostname, port);
        }
        public void Reconect(string hostname, int port)
        {
            udpClient.Close();
            udpClient.Connect(hostname, port);
        }
        public void Close() => udpClient.Close();
        public async Task<Command> ReceiveCommandAsync()
        {
            if (udpClient.Available != 0)
                return new Command(await udpClient.ReceiveAsync());
            return null;
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
