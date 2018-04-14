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
        #endregion

        public ImageServer(IImageController controller, ILoggingService logging, string[] handlersPathes)
        {
            Controller = controller;
            Logging = logging;

            foreach (var path in handlersPathes)
            {
                m_logging.Log("Creating handler for:" + path, MessageTypeEnum.INFO);
                IDirectoryHandler handler = new DirectoyHandler(m_controller, m_logging, path);
                CommandRecieved += handler.OnCommandRecieved;
                handler.StartHandleDirectory(path);
            }
        }
    }
}
