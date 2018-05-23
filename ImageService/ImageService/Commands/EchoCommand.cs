using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using Infrastructure.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class EchoCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            //Create result command.
            ConfigurationRecieveEventArgs command =
                 new ConfigurationRecieveEventArgs((int)ConfigurationEnum.Ack, args);
            //Serialize it.
            string output = JsonConvert.SerializeObject(command);
            result = true;
            return output;
        }
    }
}
