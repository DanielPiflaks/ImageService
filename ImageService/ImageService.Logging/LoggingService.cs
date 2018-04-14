
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {  
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        /// <summary>
        /// Write to log.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="type">Type of message.</param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(type, message));
        }
    }
}
