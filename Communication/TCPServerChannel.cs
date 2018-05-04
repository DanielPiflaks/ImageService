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

        public TCPServerChannel(string ip, int port, ILoggingService logging)
        {
            IP = IPAddress.Parse(ip);
            Port = port;
            Logging = logging;

            Listener = new TcpListener(IP, port);
            //Write to log.
            Logging.Log("Creating TCP Server channel", MessageTypeEnum.INFO);

            listOfSockets = new List<Socket>();
        }

        public static void StartListening()
        {
            Listener.Start();
            Logging.Log("TCP Starting to listen for clients", MessageTypeEnum.INFO);
            Socket s = Listener.AcceptSocket();
            Message newMessage = new Message();
            var k = s.Receive(newMessage.Data);
            var objectRecieved = MessageDecoder.Deserialize(newMessage);
            var t = new Thread(() => HandleClient.HandleClientRequest(objectRecieved));
            t.Start();
        }

        public static void Send(object message)
        {

        }
    }
}
