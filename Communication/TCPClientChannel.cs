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
                catch (Exception ex)
                {

                }

            });

            task.Start();
            task.Wait();
        }

        public string SendAndReceive(CommandRecievedEventArgs command)
        {
            String JsonMsgReceive = null;

            //Create new task to handle execution of command.
            Task task = new Task(() =>
            {
                try
                {
                    NetworkStream stream = TCPClient.GetStream();

                    BinaryWriter writer = new BinaryWriter(stream);
                    BinaryReader reader = new BinaryReader(stream);

                    String JsonMsgSend = JsonConvert.SerializeObject(command);
                    writer.Write(JsonMsgSend);

                    JsonMsgReceive = reader.ReadString();

                }
                catch (Exception ex)
                {

                }

            });

            task.Start();
            task.Wait();

            return JsonMsgReceive;
        }

        public string receive()
        {
            BinaryReader reader = new BinaryReader(TCPClient.GetStream());
            string JsonMsgReceive = reader.ReadString();
            return JsonMsgReceive;
        }

    }
}