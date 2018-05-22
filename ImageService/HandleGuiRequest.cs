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

namespace ImageService
{
    public class HandleGuiRequest : IHandleClient
    {
        public delegate void NotifyClients(string message);
        public static event NotifyClients NotifyAllClientsEvent;

        private Dictionary<int, ICommand> commands;
        private ILoggingService m_logging;
        private bool m_stopTask;

        public HandleGuiRequest(ILoggingService logging, ImageServer imageServer)
        {
            commands = new Dictionary<int, ICommand>()
            {
                { (int) CommandEnum.CloseCommand,  new CloseCommand() },
                { (int) CommandEnum.GetConfigCommand,  new GetConfigCommand() },
                { (int) CommandEnum.CloseHandler,  new CloseHandlerCommand(imageServer)}
            };
            m_logging = logging;
            m_stopTask = false;
        }

        public static void InvokeEvent(string message)
        {
            NotifyAllClientsEvent.Invoke(message);
        }

        public void handle(TcpClient client)
        {
            Task task = new Task(() =>
            {
                try
                {
                    while (!m_stopTask)
                    {
                        NetworkStream stream = client.GetStream();
                        BinaryReader reader = new BinaryReader(stream);
                        BinaryWriter writer = new BinaryWriter(stream);

                        string commandLine = reader.ReadString();
                        CommandRecievedEventArgs wantedCommand = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(commandLine);
                        //Check if wanted command ID is exist.
                        if (commands.ContainsKey(wantedCommand.CommandID))
                        {
                            bool result;
                            //Execute command.
                            string resultMessage = commands[wantedCommand.CommandID].Execute(wantedCommand.Args, out result);
                            writer.Write(resultMessage);
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
