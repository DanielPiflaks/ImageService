using Communication.Interfaces;
using ImageService.Logging;
using ImageService.Logging.Modal;
using Infrastructure.Event;
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

        private List<TcpClient> m_clientsListeners;
        private readonly Mutex mutex = new Mutex();

        #endregion

        public TCPServerChannel(int port, ILoggingService logging, IHandleClient handleClient)
        {
            IP = IPAddress.Parse("127.0.0.1");
            Port = port;
            Logging = logging;
            HandleClient = handleClient;
            m_clientsListeners = new List<TcpClient>();
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

                        mutex.WaitOne();
                        m_clientsListeners.Add(client);
                        mutex.ReleaseMutex();

                        HandleClient.handle(client);
                    }
                    catch (SocketException e)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            task.Start();
        }

        public void NotifyClientsOnChange(object sender, ConfigurationRecieveEventArgs e)
        {
            try
            {
                string message = JsonConvert.SerializeObject(e);

                foreach (TcpClient client in m_clientsListeners)
                {
                    new Task(() =>
                    {
                        try
                        {
                            NetworkStream stream = client.GetStream();
                            BinaryWriter writer = new BinaryWriter(stream);
                            mutex.WaitOne();
                            writer.Write(message);
                            mutex.ReleaseMutex();
                        }
                        catch (Exception ex)
                        {
                            m_clientsListeners.Remove(client);
                        }

                    }).Start();
                }
            }
            catch (Exception ex)
            {
                Logging.Log(ex.Message, MessageTypeEnum.FAIL);
            }
        }
    }
}
