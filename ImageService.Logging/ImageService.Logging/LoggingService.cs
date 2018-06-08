
using ImageService.Infrastructure.Enums;
using Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        public List<MessageRecievedEventArgs> logHistory;
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public event EventHandler<ConfigurationRecieveEventArgs> NotifyClients;

        public LoggingService()
        {
            logHistory = new List<MessageRecievedEventArgs>();
        }

        /// <summary>
        /// Write to log.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="type">Type of message.</param>
        public void Log(string message, MessageTypeEnum type)
        {
            logHistory.Add(new MessageRecievedEventArgs(type, message));
            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(type, message));

            string[] args = new string[2];
            args[0] = ((int)type).ToString();
            args[1] = message;
            ConfigurationRecieveEventArgs command =
                new ConfigurationRecieveEventArgs((int)ConfigurationEnum.NewLogMessageConfiguraton, args);
            NotifyClients?.Invoke(this, command);
        }

        public List<MessageRecievedEventArgs> GetLogHistory()
        {
            return logHistory;
        }
    }
}
