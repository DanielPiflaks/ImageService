﻿using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Event
{
    public class MessageRecievedEventArgs : EventArgs
    {
        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }
        public MessageRecievedEventArgs(MessageTypeEnum type, string message)
        {
            this.Status = type;
            this.Message = message;
        }
    }
}
