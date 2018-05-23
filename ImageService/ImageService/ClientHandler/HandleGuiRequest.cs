using Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Communication.Interfaces;
using Newtonsoft.Json;
using Infrastructure.Event;
using Infrastructure.Enums;
using ImageService.Commands;
using ImageService.Logging;
using ImageService.Server;
using ImageService.Logging.Modal;

namespace ImageService
{
    public class HandleGuiRequest : IHandleClient
    {
        public event EventHandler<ConfigurationRecieveEventArgs> NotifyClients;

        private Dictionary<int, ICommand> commands;
        private ILoggingService m_logging;
        private bool m_stopTask;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logging">logging</param>
        /// <param name="imageServer">Image server.</param>
        public HandleGuiRequest(ILoggingService logging, ImageServer imageServer)
        {
            //Create dictionary from enum int to ICommand
            commands = new Dictionary<int, ICommand>()
            {
                { (int) CommandEnum.GetConfigCommand,  new GetConfigCommand() },
                { (int) CommandEnum.CloseHandler,  new CloseHandlerCommand(imageServer, this)},
                { (int) CommandEnum.LogCommand, new LogCommand(logging) },
                { (int) CommandEnum.EchoCommand, new EchoCommand() }
            };
            m_logging = logging;
            m_stopTask = false;
        }
        /// <summary>
        /// Invokes event.
        /// </summary>
        /// <param name="message">Message to send to all clients.</param>
        public void InvokeEvent(ConfigurationRecieveEventArgs message)
        {
            NotifyClients?.Invoke(this, message);
        }
        /// <summary>
        /// Handles
        /// </summary>
        /// <param name="client">Client to handle.</param>
        public void handle(TcpClient client)
        {
            //Create task for handle client.
            Task task = new Task(() =>
            {
                try
                {
                    //While client is working.
                    while (!m_stopTask)
                    {
                        //Get stream.
                        NetworkStream stream = client.GetStream();
                        //Get reader from stream.
                        BinaryReader reader = new BinaryReader(stream);
                        //Get client request.
                        string commandLine = reader.ReadString();
                        //Deserialize it.
                        CommandRecievedEventArgs wantedCommand = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(commandLine);
                        //Check if wanted command ID is exist.
                        if (commands.ContainsKey(wantedCommand.CommandID))
                        {
                            //Get writer from stream.
                            BinaryWriter writer = new BinaryWriter(stream);
                            bool result;
                            //Execute command.
                            string resultMessage = commands[wantedCommand.CommandID].Execute(wantedCommand.Args, out result);
                            //Write command or write to log the result by commandID
                            if (((CommandEnum)wantedCommand.CommandID != CommandEnum.CloseHandler) && (((CommandEnum)wantedCommand.CommandID != CommandEnum.CloseCommand)))
                            {
                                writer.Write(resultMessage);
                            }
                            else
                            {
                                m_logging.Log(resultMessage, MessageTypeEnum.INFO);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    m_logging.Log("Problem in handleing client - " + e.Message, Logging.Modal.MessageTypeEnum.FAIL);
                }

            });
            //Start task.
            task.Start();
        }
    }
}
