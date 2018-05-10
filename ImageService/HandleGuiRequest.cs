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
using Infrastructure.Enums;
using ImageService.Commands;
using Infrastructure.Modal;
using ImageService.Logging;

namespace ImageService
{
    public class HandleGuiRequest : IHandleClient
    {
        private Dictionary<int, ICommand> commands;
        private ILoggingService m_logging;
        private bool m_stopTask;

        public HandleGuiRequest(ILoggingService logging)
        {
            commands = new Dictionary<int, ICommand>()
            {
                { (int) CommandEnum.CloseCommand,  new CloseCommand() },
                { (int) CommandEnum.GetConfigCommand,  new GetConfigCommand() },
                { (int) CommandEnum.LogCommand,  new CloseCommand() }
            };
            m_logging = logging;
            m_stopTask = false;
        }

        public void handle(TcpClient client)
        {
            Task task = new Task(() =>
            {
                try
                {
                    while (!m_stopTask)
                    {
                        using (NetworkStream stream = client.GetStream())
                        using (BinaryReader reader = new BinaryReader(stream))
                        using (BinaryWriter writer = new BinaryWriter(stream))
                        {
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
