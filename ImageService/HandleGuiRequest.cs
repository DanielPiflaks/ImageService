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

namespace ImageService
{
    public class HandleGuiRequest : IHandleClient
    {
        private Dictionary<int, ICommand> commands;

        public HandleGuiRequest()
        {
            commands = new Dictionary<int, ICommand>()
            {
                { (int) CommandEnum.CloseCommand,  new CloseCommand() },
                { (int) CommandEnum.GetConfigCommand,  new GetConfigCommand() },
                { (int) CommandEnum.LogCommand,  new CloseCommand() }
            };
        }

        public void handle(TCPServerChannel tcpServerChannel, TcpClient client)
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
                    //Create new task to handle execution of command.
                    Task task = new Task(() =>
                    {
                        bool result;
                        //Execute command.
                        string resultMessage = commands[wantedCommand.CommandID].Execute(wantedCommand.Args, out result);
                        writer.Write(resultMessage);
                    });
                    //Start task.
                    task.Start();
                }
            }
        }
    }
}
