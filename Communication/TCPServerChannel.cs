﻿using ImageService.Logging;
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

        public Tuple<CommandMessage, Stream> StartListening()
        {
            Logging.Log("Check", MessageTypeEnum.INFO);
            TcpClient client = null;
            try
            {
                client = Listener.AcceptTcpClient();
            }
            catch (Exception e)
            {
                Logging.Log(e.Message, MessageTypeEnum.INFO);
            }
            
            Logging.Log("TCP Starting to listen for clients", MessageTypeEnum.INFO);

            NetworkStream stream = client.GetStream();
            CommandMessage cmdMessage;
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                string commandLine = reader.ReadString();
                cmdMessage = JsonConvert.DeserializeObject<CommandMessage>(commandLine);
            }
        
            return Tuple.Create<CommandMessage, Stream>(cmdMessage, stream);
        }

        public void SendMessage(object msg, Stream stream)
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                String JsonMsg = JsonConvert.SerializeObject(msg);
                writer.Write(JsonMsg);
            }
        }
    }
}
