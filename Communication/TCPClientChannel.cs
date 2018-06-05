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
using Communication.Interfaces;

namespace Communication
{
    public class TCPClientChannel : ITCPClientChannel
    {
        //Global parameters.
        public const string IP = "127.0.0.1";
        public const int PORT = 8000;

        //members.
        private static TCPClientChannel clientTcp;
        private TcpClient m_tcpClient;
        private bool m_stopListening;
        public delegate void NotifyIncomingMessage(string message);
        public event NotifyIncomingMessage NotifyMessage;
        private static readonly Mutex mutexCtorLock = new Mutex();

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
            //Create IPEndPoint with local ip and port.
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), PORT);
            //Create TcpClient object.
            TCPClient = new TcpClient();
            //Connect.
            TCPClient.Connect(ep);
            m_stopListening = false;
        }

        /// <summary>
        /// Get TCP client channel as our object is singelton.
        /// </summary>
        /// <returns>TCPClientChannel object.</returns>
        public static TCPClientChannel GetTCPClientChannel()
        {
            //Try to get tcp client channel.
            try
            {
                //Lock mutex.
                //mutexCtorLock.WaitOne();
                if (clientTcp == null)
                {
                    //If object is not exists, create one.
                    clientTcp = new TCPClientChannel();
                }
                //Unlock mutex.
                //mutexCtorLock.ReleaseMutex();
                return clientTcp;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Send command to server.
        /// </summary>
        /// <param name="command">Command to send.</param>
        public void Send(CommandRecievedEventArgs command)
        {
            //Create new thread for sending.
            Task task = new Task(() =>
            {
                try
                {
                    //Get stream.
                    NetworkStream stream = TCPClient.GetStream();
                    //Get writer from stream.
                    BinaryWriter writer = new BinaryWriter(stream);
                    //Serialize object using JsonConvert.
                    String JsonMsgSend = JsonConvert.SerializeObject(command);
                    //Send command
                    writer.Write(JsonMsgSend);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            });

            task.Start();
            task.Wait();
        }

        /// <summary>
        /// Recieve from server.
        /// </summary>
        /// <returns></returns>
        public string Receive()
        {
            //Get stream.
            NetworkStream stream = TCPClient.GetStream();
            //Get reader from stream.
            BinaryReader reader = new BinaryReader(stream);
            //Read message from server.
            string message = reader.ReadString();
            return message;
        }

        /// <summary>
        /// Send to server and recieve a message from it.
        /// </summary>
        /// <param name="command">Command to send.</param>
        /// <returns></returns>
        public string SendAndReceive(CommandRecievedEventArgs command)
        {
            String message = null;

            //Create task for sendinf.
            Task task = new Task(() =>
            {
                try
                {
                    //Use Send func for sending.
                    Send(command);
                    //Recieve response from server.
                    message = Receive();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            });

            task.Start();
            task.Wait();
            //Return message.
            return message;
        }

        /// <summary>
        /// Listen to server.
        /// </summary>
        public void ListenToServer()
        {
            String message = null;

            //Create task to handle listening.
            Task task = new Task(() =>
            {
                try
                {
                    //While there is no request to stop listening.
                    while (!m_stopListening)
                    {
                        //Receive message from server.
                        message = Receive();
                        //Notify that message recieved.
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