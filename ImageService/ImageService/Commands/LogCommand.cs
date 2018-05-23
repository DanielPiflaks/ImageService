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
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logging">Logging.</param>
        public LogCommand(ILoggingService logging)
        {
            m_logging = logging;
        }


        public string Execute(string[] args, out bool result)
        {
            //Get log history.
            List<MessageRecievedEventArgs> logHistory = m_logging.GetLogHistory();
            //Count log history size.
            int numberOfMessages = logHistory.Count;
            //Create string array.
            string[] logHistoryArray = new string[numberOfMessages * 2];
            int counter = 0;
            //Put all messages into array.
            for (int i = 0; i < logHistoryArray.Length; i = i + 2, counter++)
            {
                //First place is enum number.
                logHistoryArray[i] = ((int)logHistory[counter].Status).ToString();
                //Second place is message.
                logHistoryArray[i + 1] = logHistory[counter].Message;
            }
            //Create result command.
            ConfigurationRecieveEventArgs command = new ConfigurationRecieveEventArgs((int)ConfigurationEnum.LogHistory, logHistoryArray);
            //Serialize it.
            string output = JsonConvert.SerializeObject(command);
            result = true;
            return output;
        }
    }
}
