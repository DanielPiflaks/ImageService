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
        private HandleGuiRequest m_handleGui;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="imageServer">Image server.</param>
        /// <param name="handleGui">Handles gui client.</param>
        public CloseHandlerCommand(ImageServer imageServer, HandleGuiRequest handleGui)
        {
            m_imageServer = imageServer;
            m_handleGui = handleGui;
        }
        /// <summary>
        /// Execute command.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            //Get handler to remove from args.
            string removeHandler = args[0];
            //Remove wanted handler.
            result = m_imageServer.RemoveHandler(removeHandler);
            string resultString;
            //Check result and create result string accordingly.
            if (result)
            {
                resultString = "Handler " + removeHandler + " removed";
            }
            else
            {
                resultString = "Handler " + removeHandler + " can't be removed. Check log.";
            }
            //Create result command.
            ConfigurationRecieveEventArgs command =
                new ConfigurationRecieveEventArgs((int)ConfigurationEnum.RemoveHandler, args);
            //Serialize it.
            string output = JsonConvert.SerializeObject(command);
            //Invoke with result command.
            //m_handleGui.InvokeEvent(command);
            return output;
        }
    }
}
