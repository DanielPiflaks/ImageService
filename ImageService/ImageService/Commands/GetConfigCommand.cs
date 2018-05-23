using ImageService.Infrastructure.Enums;
using Infrastructure.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            try
            {
                List<string> sendArgs = new List<string>();
                //Get parameters from app config
                sendArgs.Add(ConfigurationManager.AppSettings.Get("OutputDir"));
                sendArgs.Add(ConfigurationManager.AppSettings.Get("SourceName"));
                sendArgs.Add(ConfigurationManager.AppSettings.Get("LogName"));
                sendArgs.Add(ConfigurationManager.AppSettings.Get("ThumbnailSize"));
                //Split handlers by ;
                string[] handlers = HandlerListManager.GetHandlerListManager().Handlers;
                //Convert string array into list.
                for (int i = 0; i < handlers.Length; i++)
                {
                    sendArgs.Add(handlers[i]);
                }
                //Create recieve command.
                ConfigurationRecieveEventArgs configurationEvent =
                    new ConfigurationRecieveEventArgs((int)ConfigurationEnum.SettingsConfiguration, sendArgs.ToArray());
                //Serialize it.
                string output = JsonConvert.SerializeObject(configurationEvent);
                result = true;
                return output;
            }
            catch (Exception)
            {
                result = false;
                return "Couldn't get settings propetries.";
            }
        }
    }
}
