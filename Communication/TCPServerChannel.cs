using ImageService.Logging;
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Communication
{
    public class TCPServerChannel
    {
        #region Properties
        // Create a new Mutex. The creating thread does not own the mutex.
        private static Mutex mut = new Mutex();
        private static List<Socket> listOfSockets;
        private static TcpListener Listener;
        private static ILoggingService Logging;

        private int m_port;
        public int Port
        {
            get { return m_port; }
            set { m_port = value; }
        }

        private IPAddress m_ip;
        public IPAddress IP
        {
            get { return m_ip; }
            set { m_ip = value; }
        }

        #endregion

        public TCPServerChannel(int port, ILoggingService logging)
        {
            IP = IPAddress.Parse(GetLocalIPAddress());
            Port = port;
            Logging = logging;

            Listener = new TcpListener(IP, port);
            //Write to log.
            Logging.Log("Creating TCP Server channel", MessageTypeEnum.INFO);

            listOfSockets = new List<Socket>();
        }

        public Tuple<Message, Socket> StartListening()
        {
            Listener.Start();
            Logging.Log("TCP Starting to listen for clients", MessageTypeEnum.INFO);
            Socket s = Listener.AcceptSocket();
            Message newMessage = new Message();
            s.Receive(newMessage.Data);
            return Tuple.Create(newMessage, s);
        }

        public void SendMessage(Message msg, Socket s)
        {
            s.Send(msg.Data, 0);
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
