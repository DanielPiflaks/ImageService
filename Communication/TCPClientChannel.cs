using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;

namespace Communication
{
    class TCPClientChannel
    {
        public const string IP = "127.0.0.1";
        public const int PORT = 8888;

        //members.
        private static TCPClientChannel clientTcp;
        private TcpClient m_tcpClient;
        private Stream stm;

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
            TCPClient = new TcpClient();
            TCPClient.Connect(IP, PORT);
            stm = TCPClient.GetStream();
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

        public void Send(CommandEnum cmd, String[] args)
        {
            
        }

        public static CommandMessage Serialize(object anySerializableObject)
        {
            using (var memoryStream = new MemoryStream())
            {
                (new BinaryFormatter()).Serialize(memoryStream, anySerializableObject);
                return new CommandMessage { Data = memoryStream.ToArray() };
            }
        }
    }
}