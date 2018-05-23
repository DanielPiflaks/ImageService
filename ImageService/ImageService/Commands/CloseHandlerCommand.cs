﻿using ImageService.Commands;
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
        public CloseHandlerCommand(ImageServer imageServer, HandleGuiRequest handleGui)
        {
            m_imageServer = imageServer;
            m_handleGui = handleGui;
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

            m_handleGui.InvokeEvent(command);
            return resultString;
        }
    }
}
