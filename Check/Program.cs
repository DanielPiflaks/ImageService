using Communication;
using ImageService.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Check
{
    class Program
    {
        static void Main(string[] args)
        {
            TCPServerChannel tcpServerChannel = new TCPServerChannel(8000, null);

            var t = new Thread(() => ListenToClients(tcpServerChannel));
            t.Start();
        }

        public static void ListenToClients(TCPServerChannel tcpServerChannel)
        {
            while (true)
            {
                object receivedMsg = tcpServerChannel.StartListening();
                //var t = new Thread(() => HandleGuiRequest.handle(objectRecieved, tcpServerChannel, receivedMsg.Item2));
                //t.Start();
            }
        }
    }
}
