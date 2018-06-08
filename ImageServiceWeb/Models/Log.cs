﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ImageService.Infrastructure.Enums;

namespace ImageServiceWeb.Models
{
    public class Log: EventArgs
    {
        [Required]
        [Display(Name = "Status")]
        public MessageTypeEnum Status { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }

        public Log(MessageTypeEnum type, string message)
        {
            this.Status = type;
            this.Message = message;
        }

        public enum MessageType
        {
            INFO,
            WARNING,
            FAIL
        }
    }
}