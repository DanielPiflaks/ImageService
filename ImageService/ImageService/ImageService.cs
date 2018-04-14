using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ImageService.Server;
using ImageService.Controller;
using ImageService.Modal;
using ImageService.Logging;
using System.Configuration;
using ImageService.Logging.Modal;

namespace ImageService
{
    public partial class ImageService : ServiceBase
    {
        #region Members
        private ImageServer m_imageServer;
        private IImageServiceModal m_imageServiceModal;
        private IImageController m_controller;
        private ILoggingService m_loggingService;
        #endregion

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        private int eventId = 1;

        public ImageService(string[] args)
        {
            try
            {
                InitializeComponent();
                //Get settings from app.config.
                ServiceSettings serviceSettings = ServiceSettings.GetServiceSettings();
                //Set service settings.
                string eventSourceName = serviceSettings.SourceName;
                //Set log name.
                string logName = serviceSettings.LogName;
                if (args.Count() > 0)
                {
                    eventSourceName = args[0];
                }
                if (args.Count() > 1)
                {
                    logName = args[1];
                }
                eventLog1 = new System.Diagnostics.EventLog();
                if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
                {
                    System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
                }
                eventLog1.Source = eventSourceName;
                eventLog1.Log = logName;

                //Create image service modal.
                m_imageServiceModal = new ImageServiceModal(serviceSettings.OutputDir, serviceSettings.ThumbnailSize);
                //Create logging service.
                m_loggingService = new LoggingService();
                m_loggingService.MessageRecieved += WriteToLog;
                //Create controller.
                m_controller = new ImageController(m_imageServiceModal);
                //Create image server.
                m_imageServer = new ImageServer(m_controller, m_loggingService, serviceSettings.Handlers);
                m_loggingService.Log("Image service created", Logging.Modal.MessageTypeEnum.INFO);
            }
            catch
            {
                m_loggingService.Log("Failed creating image service", Logging.Modal.MessageTypeEnum.FAIL);
            }
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("In OnStart");
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }

        protected override void OnStop()
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog1.WriteEntry("In onStop.");
            //Close server.
            if (m_imageServer != null)
            {
                m_imageServer.OnCloseServer();
            }
 
            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            m_loggingService.Log("Image service closed.", Logging.Modal.MessageTypeEnum.INFO);
        }

        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
        }

        protected override void OnPause()
        {
            eventLog1.WriteEntry("In OnPause.");
        }

        protected override void OnShutdown()
        {
            eventLog1.WriteEntry("In OnShutdown.");
        }

        private void WriteToLog(object sender, MessageRecievedEventArgs e)
        {
            EventLogEntryType messageType;

            switch (e.Status)
            {
                case MessageTypeEnum.INFO:
                    messageType = EventLogEntryType.Information;
                    break;
                case MessageTypeEnum.WARNING:
                    messageType = EventLogEntryType.Warning;
                    break;
                case MessageTypeEnum.FAIL:
                    messageType = EventLogEntryType.Error;
                    break;
                default:
                    messageType = EventLogEntryType.Information;
                    break;
            }

            eventLog1.WriteEntry(e.Message, messageType);
        }

        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
            eventLog1.WriteEntry(e.Entry.ToString());
        }

    }
}
