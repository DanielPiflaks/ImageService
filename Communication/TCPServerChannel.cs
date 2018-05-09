using Communication.Interfaces;
using ImageService.Logging;
using ImageService.Logging.Modal;
using Newtonsoft.Json;
//using Newtonsoft.Json;
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
        private static TcpListener Listener;
        private static ILoggingService Logging;

        private IHandleClient m_handleClient;
        public IHandleClient HandleClient
        {
            get { return m_handleClient; }
            set { m_handleClient = value; }
        }


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

        public TCPServerChannel(int port, ILoggingService logging, IHandleClient handleClient)
        {
            IP = IPAddress.Parse("127.0.0.1");
            Port = port;
            Logging = logging;
            HandleClient = handleClient;
        }

        public void SendMessage(object msg, TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                String JsonMsg = JsonConvert.SerializeObject(msg);
                writer.Write(JsonMsg);
            }
        }

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IP, Port);
            Listener = new TcpListener(ep);
            Listener.Start();
            //Write to log.
            Logging.Log("Creating TCP Server channel", MessageTypeEnum.INFO);

            Listener.Start();
            Logging.Log("Waiting for connections...", MessageTypeEnum.INFO);

            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = Listener.AcceptTcpClient();
                        Logging.Log("Got new connection", MessageTypeEnum.INFO);

                        //CommandRecieveEventArgs cmdMessage = null;
                        /*using (NetworkStream stream = client.GetStream())
                        using (BinaryReader reader = new BinaryReader(stream))
                        using (BinaryWriter writer = new BinaryWriter(stream))
                        {
                            string commandLine = reader.ReadString();
                            cmdMessage = JsonConvert.DeserializeObject<CommandMessage>(commandLine);
                        }*/

                        HandleClient.handle(this, client);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            task.Start();
        }

    }
}
