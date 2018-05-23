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
    public class TCPServerChannel : ITCPServerChannel
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
        //private object locker = new object();

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="port">Port number</param>
        /// <param name="logging">Logging.</param>
        /// <param name="handleClient">Client handler class.</param>
        public TCPServerChannel(int port, ILoggingService logging, IHandleClient handleClient)
        {   
            //Set ip to be local host.
            IP = IPAddress.Parse("127.0.0.1");
            Port = port;
            Logging = logging;
            HandleClient = handleClient;
            m_clientsListeners = new List<TcpClient>();
        }

        /// <summary>
        /// Starts tcp server channel.
        /// </summary>
        public void Start()
        {
            //Create IP end point.
            IPEndPoint ep = new IPEndPoint(IP, Port);
            //Create tcp listener.
            Listener = new TcpListener(ep);
            //Write to log.
            Logging.Log("Creating TCP Server channel", MessageTypeEnum.INFO);
            //Start to listen.
            Listener.Start();
            Logging.Log("Waiting for connections...", MessageTypeEnum.INFO);
            //Create task for listening to clients.
            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        //Accept tcp client.
                        TcpClient client = Listener.AcceptTcpClient();
                        Logging.Log("Got new connection", MessageTypeEnum.INFO);
                        //Add client to list of clients.
                        mutex.WaitOne();
                        m_clientsListeners.Add(client);
                        mutex.ReleaseMutex();
                        //Handle client.
                        Logging.Log("Start handleing client.", MessageTypeEnum.INFO);
                        HandleClient.handle(client);
                    }
                    catch (SocketException e)
                    {
                        Logging.Log("Porblem in accepting client, exception is: " + e.Message, MessageTypeEnum.WARNING);
                    }
                }
            });
            task.Start();
        }
        /// <summary>
        /// Notify all clients about change.
        /// </summary>
        /// <param name="sender">Who wants to notify.</param>
        /// <param name="e">Parameter for notification.</param>
        public void NotifyClientsOnChange(object sender, ConfigurationRecieveEventArgs e)
        {
            try
            {
                //Serialize object in json format for sending it.
                string message = JsonConvert.SerializeObject(e);
                //Notify all clients in client list.
                foreach (TcpClient client in m_clientsListeners)
                {
                    new Task(() =>
                    {
                        try
                        {
                            //Get streamer.
                            NetworkStream stream = client.GetStream();
                            BinaryWriter writer = new BinaryWriter(stream);
                            mutex.WaitOne();
                            //Write message.
                            writer.Write(message);
                            mutex.ReleaseMutex();
                            //writer.Flush();
                        }
                        catch (Exception ex)
                        {
                            mutex.WaitOne();
                            m_clientsListeners.Remove(client);
                            mutex.ReleaseMutex();
                            Logging.Log("Removing client from list because: " + ex.Message, MessageTypeEnum.WARNING);
                        }

                    }).Start();
                }
            }
            catch (Exception ex)
            {
                Logging.Log("Exception while notify all client about update: " + ex.Message, MessageTypeEnum.FAIL);
            }
        }
    }
}
