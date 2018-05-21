using ImageService.Commands;
using Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            try
            {
                //Split handlers by ;
                string[] handlers = HandlerListManager.GetHandlerListManager().Handlers;
                string outputDir = ConfigurationManager.AppSettings.Get("OutputDir");
                string sourceName = ConfigurationManager.AppSettings.Get("SourceName");
                string logName = ConfigurationManager.AppSettings.Get("LogName");
                int thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings.Get("ThumbnailSize"));

                SettingsParams parameters = new SettingsParams(handlers, outputDir, sourceName, logName, thumbnailSize);
                string output = JsonConvert.SerializeObject(parameters);
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
