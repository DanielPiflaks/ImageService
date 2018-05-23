using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Enums;
using Newtonsoft.Json;
using Infrastructure.Event;
using System.Threading;

namespace Communication
{
    public class TCPClientChannel
    {
        public const string IP = "127.0.0.1";
        public const int PORT = 8000;
    
        //members.
        private static TCPClientChannel clientTcp;
        private TcpClient m_tcpClient;
        private bool m_stopListening;
        public delegate void NotifyIncomingMessage(string message);
        public event NotifyIncomingMessage NotifyMessage;
        private readonly Mutex mutex = new Mutex();

        /// <summary>
        /// properties.
        /// </summary>
        public TcpClient TCPClient
        {
            get { return m_tcpClient; }
            set { m_tcpClient = value; }
        }

        /// <summary>
        /// constructor.
        /// </summary>
        private TCPClientChannel()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), PORT);
            TCPClient = new TcpClient();
            TCPClient.Connect(ep);
            m_stopListening = false;
        }

        public static TCPClientChannel GetTCPClientChannel()
        {
            try
            {
                if (clientTcp == null)
                {
                    clientTcp = new TCPClientChannel();
                }
                return clientTcp;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Send(CommandRecievedEventArgs command)
        {
            Task task = new Task(() =>
            {
                try
                {
                    NetworkStream stream = TCPClient.GetStream();
                    BinaryWriter writer = new BinaryWriter(stream);
                    String JsonMsgSend = JsonConvert.SerializeObject(command);
                    writer.Write(JsonMsgSend);
                }
                catch (Exception e)
                {

                }

            });

            task.Start();
            task.Wait();
        }

        public string Receive()
        {
            NetworkStream stream = TCPClient.GetStream();
            BinaryReader reader = new BinaryReader(stream);

            //mutex.WaitOne();
            string message = reader.ReadString();
            //mutex.ReleaseMutex();
            return message;
        }

        public string SendAndReceive(CommandRecievedEventArgs command)
        {
            String message = null;

            //Create new task to handle execution of command.
            Task task = new Task(() =>
            {
                try
                {
                    Send(command);
                    message = Receive();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            });

            task.Start();
            task.Wait();

            return message;
        }
      
        public void ListenToServer()
        {
            String message = null;

            //Create new task to handle execution of command.
            Task task = new Task(() =>
            {
                try
                {
                    while (!m_stopListening)
                    {
                        message = Receive();
                        NotifyMessage?.Invoke(message);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message + message);
                }
            
            });
            task.Start();
        }

    }
}