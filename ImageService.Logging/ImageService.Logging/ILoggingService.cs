using ImageService.Infrastructure.Enums;
using Infrastructure.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public interface ILoggingService
    {
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        event EventHandler<ConfigurationRecieveEventArgs> NotifyClients;

        // Logging the Message
        void Log(string message, MessageTypeEnum type);
        List<MessageRecievedEventArgs> GetLogHistory();        
    }
}
