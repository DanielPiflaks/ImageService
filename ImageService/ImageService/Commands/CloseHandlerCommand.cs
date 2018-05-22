using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Server;
using Infrastructure.Enums;
using Infrastructure.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class CloseHandlerCommand : ICommand
    {
        private ImageServer m_imageServer;
        public CloseHandlerCommand(ImageServer imageServer)
        {
            m_imageServer = imageServer;
        }

        public string Execute(string[] args, out bool result)
        {
            string removeHandler = args[0];
            result = m_imageServer.RemoveHandler(removeHandler);
            string resultString;
            if (result)
            {
                resultString = "Handler " + removeHandler + " removed";

            }
            else
            {
                resultString = "Handler " + removeHandler + " can't be removed. Check log.";
            }

            ConfigurationRecieveEventArgs command =
                new ConfigurationRecieveEventArgs((int)ConfigurationEnum.RemoveHandler, args);
            string message = JsonConvert.SerializeObject(command);

            HandleGuiRequest.InvokeEvent(message);
            return resultString;
        }
    }
}
