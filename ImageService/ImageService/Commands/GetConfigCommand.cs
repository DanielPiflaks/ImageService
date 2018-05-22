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
                //Split handlers by ;
                sendArgs.Add(ConfigurationManager.AppSettings.Get("OutputDir"));
                sendArgs.Add(ConfigurationManager.AppSettings.Get("SourceName"));
                sendArgs.Add(ConfigurationManager.AppSettings.Get("LogName"));
                sendArgs.Add(ConfigurationManager.AppSettings.Get("ThumbnailSize"));

                string[] handlers = HandlerListManager.GetHandlerListManager().Handlers;
                for (int i = 0; i < handlers.Length; i++)
                {
                    sendArgs.Add(handlers[i]);
                }

                ConfigurationRecieveEventArgs configurationEvent =
                    new ConfigurationRecieveEventArgs((int)ConfigurationEnum.SettingsConfiguration, sendArgs.ToArray());
                string output = JsonConvert.SerializeObject(configurationEvent);
                result = true;
                return output;
            }
            catch (Exception e)
            {
                result = false;
                return "Couldn't get settings propetries.";
            }
        }
    }
}
