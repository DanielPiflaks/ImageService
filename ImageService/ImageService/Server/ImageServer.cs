using ImageService.Controller;
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
        // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        public event EventHandler<DirectoryCloseEventArgs> CloseServer;
        #endregion
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controller">Image controller.</param>
        /// <param name="logging">Logging</param>
        /// <param name="handlersPathes">Handlera pathes.</param>
        public ImageServer(IImageController controller, ILoggingService logging, string[] handlersPathes)
        {
            Controller = controller;
            Logging = logging;
            //For each path in handlers pathes.
            foreach (var path in handlersPathes)
            {
                //Write to log.
                m_logging.Log("Creating handler for: " + path, MessageTypeEnum.INFO);
                //Create handler.
                IDirectoryHandler handler = new DirectoyHandler(m_controller, m_logging, path);
                //Add events.
                CommandRecieved += handler.OnCommandRecieved;
                CloseServer += handler.CloseHandler;
                //Start handle directory.
                handler.StartHandleDirectory(path);
            }
        }

        /// <summary>
        /// When closing server.
        /// </summary>
        public void OnCloseServer()
        {
            //Close handlers.
            CloseServer?.Invoke(this, null);
            //Write to log.
            m_logging.Log("Server closed", MessageTypeEnum.INFO);
        }
    }
}
