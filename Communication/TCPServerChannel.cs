using ImageService.Logging;
using ImageService.Logging.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
            IP = IPAddress.Parse("0.0.0.0");
            Port = port;
            Logging = logging;
            IPEndPoint ep = new IPEndPoint(IP, port);
            Listener = new TcpListener(ep);
            Listener.Start();
            //Write to log.
            //Logging.Log("Creating TCP Server channel", MessageTypeEnum.INFO);

        }

        public Tuple<Message, Socket> StartListening()
        {
            TcpClient client = Listener.AcceptTcpClient();
            //Logging.Log("TCP Starting to listen for clients", MessageTypeEnum.INFO);

            Message newMessage = new Message();

            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                string commandLine = reader.ReadString();
                CommandMessage cmdMessage = JsonConvert.DeserializeObject<CommandMessage>(commandLine);

                Console.WriteLine("Number accepted");
                //num *= 2;
                //writer.Write(num);
            }

            Socket s = Listener.AcceptSocket();
            
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
