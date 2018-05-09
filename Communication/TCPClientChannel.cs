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
using Infrastructure.Modal;

namespace Communication
{
    public class TCPClientChannel
    {
        public const string IP = "127.0.0.1";
        public const int PORT = 8000;

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
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), PORT);
            TcpClient client = new TcpClient();
            client.Connect(ep);

            stm = client.GetStream();
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

        /*public void Send(CommandEnum cmd, List<String> args)
        {
            using (BinaryWriter writer = new BinaryWriter(stm))
            {
                CommandRecievedEventArgs msg = new CommandRecievedEventArgs(cmd, args);
                String JsonMsg = JsonConvert.SerializeObject(msg);
                writer.Write(JsonMsg);
            }
        }*/

        public string SendAndReceive(CommandRecievedEventArgs command)
        {
            //Send(cmd, args);
            //return receive();

            BinaryWriter writer = new BinaryWriter(stm);
            BinaryReader reader = new BinaryReader(stm);

            String JsonMsgSend = JsonConvert.SerializeObject(command);
            writer.Write(JsonMsgSend);

            String JsonMsgReceive = reader.ReadString();
            return JsonMsgReceive;
        }

        public string receive()
        {
            using (BinaryReader reader = new BinaryReader(stm))
            {
                String JsonMsg = reader.ReadString();
                return JsonMsg;
            }
        }

    }
}