using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using Infrastructure.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class LogCommand : ICommand
    {
        private ILoggingService m_logging;

        public LogCommand(ILoggingService logging)
        {
            m_logging = logging;
        }


        public string Execute(string[] args, out bool result)
        {
            List<MessageRecievedEventArgs> logHistory = m_logging.GetLogHistory();
            int numberOfMessages = logHistory.Count;
            string[] logHistoryArray = new string[numberOfMessages * 2];
            int counter = 0;
            for (int i = 0; i < logHistoryArray.Length; i = i + 2, counter++)
            {
                logHistoryArray[i] = ((int)logHistory[counter].Status).ToString();
                logHistoryArray[i + 1] = logHistory[counter].Message;
            }

            ConfigurationRecieveEventArgs command = new ConfigurationRecieveEventArgs((int)ConfigurationEnum.LogHistory, logHistoryArray);
            string output = JsonConvert.SerializeObject(command);
            result = true;
            return output;
        }
    }
}
