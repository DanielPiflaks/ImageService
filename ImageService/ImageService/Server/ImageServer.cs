﻿using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ImageService.Logging.Modal;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        public IImageController Controller
        {
            get
            {
                return m_controller;
            }
            set
            {
                m_controller = value;
            }
        }
        public ILoggingService Logging
        {
            get
            {
                return m_logging;
            }
            set
            {
                m_logging = value;
            }
        }
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        public event EventHandler<DirectoryCloseEventArgs> CloseServer;
        #endregion

        public ImageServer(IImageController controller, ILoggingService logging, string[] handlersPathes)
        {
            Controller = controller;
            Logging = logging;

            foreach (var path in handlersPathes)
            {
                try
                {
                    m_logging.Log("Creating handler for:" + path, MessageTypeEnum.INFO);
                    IDirectoryHandler handler = new DirectoyHandler(m_controller, m_logging, path);
                    CommandRecieved += handler.OnCommandRecieved;
                    CloseServer += handler.CloseHandler;
                    handler.StartHandleDirectory(path);
                    m_logging.Log("Succeeded creating handler for :" + path, MessageTypeEnum.INFO);
                }
                catch
                {
                    m_logging.Log("Failed creating handler for :" + path, MessageTypeEnum.FAIL);
                }
            }
        }

        public void OnCloseServer()
        {
            CloseServer?.Invoke(this, null);
            m_logging.Log("Succeeded closing server", MessageTypeEnum.INFO);
        }
    }
}
