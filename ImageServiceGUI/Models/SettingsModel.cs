using System;
using System.Collections.Generic;
using Infrastructure.Enums;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;
using Newtonsoft.Json;
using Infrastructure;
using Infrastructure.Event;
using System.Collections.ObjectModel;
using System.Windows.Data;
using ImageService.Infrastructure.Enums;
using System.Windows;

namespace ImageServiceGUI.Models
{
    class SettingsModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Properties
        public ObservableCollection<string> Handlers { get; set; }

        private string m_outputDir;
        public string OutputDir
        {
            get { return m_outputDir; }
            set
            {
                m_outputDir = value;
                OnPropertyChanged("OutputDir");
            }
        }

        private string m_sourceName;
        public string SourceName
        {
            get { return m_sourceName; }
            set
            {
                m_sourceName = value;
                OnPropertyChanged("SourceName");
            }
        }

        private string m_logName;
        public string LogName
        {
            get { return m_logName; }
            set
            {
                m_logName = value;
                OnPropertyChanged("LogName");
            }
        }

        private string m_thumbnailSize;
        public string ThumbnailSize
        {
            get { return m_thumbnailSize; }
            set
            {
                m_thumbnailSize = value;
                OnPropertyChanged("ThumbnailSize");
            }
        }
        #endregion
        /// <summary>
        /// Constructor.
        /// </summary>
        public SettingsModel()
        {
            //Create command.
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, null, "");
            //Add function to event.
            TCPClientChannel.GetTCPClientChannel().NotifyMessage += UpdateByNotification;
            //Send command.
            string settingsMsg = TCPClientChannel.GetTCPClientChannel().SendAndReceive(command);
            //Update notification.
            UpdateByNotification(settingsMsg);
        }

        /// <summary>
        /// Update settings model by given message.
        /// </summary>
        /// <param name="message"></param>
        public void UpdateByNotification(string message)
        {

            try
            {
                //Deserialize message.
                ConfigurationRecieveEventArgs configurationNotify =
                    JsonConvert.DeserializeObject<ConfigurationRecieveEventArgs>(message);
                //Set by given enum ID.
                switch ((ConfigurationEnum)configurationNotify.ConfigurationID)
                {
                    case ConfigurationEnum.SettingsConfiguration:
                        //Set settings by arguments.
                        SetSettings(configurationNotify.Args);
                        break;
                    case ConfigurationEnum.RemoveHandler:
                        //Check if wanted handler to remove is in Handlers and remove it.
                        if (Handlers.Contains(configurationNotify.Args[0]))
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            { Handlers.Remove(configurationNotify.Args[0]); }));
                        }

                        break;
                    default:
                        break;
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Sets settings.
        /// </summary>
        /// <param name="settings">Given settings.</param>
        public void SetSettings(string[] settings)
        {
            Handlers = new ObservableCollection<string>();
            //Get all settings from array.
            OutputDir = settings[0];
            ThumbnailSize = settings[1];
            LogName = settings[2];
            SourceName = settings[3];
            //Add handlers to observable collection.
            for (int i = 4; i < settings.Length; i++)
            {
                Handlers.Add(settings[i]);
            }
        }
        /// <summary>
        /// Remove handler from observable collection.
        /// </summary>
        /// <param name="handler">Handler to remove.</param>
        public void RemoveHandler(string handler)
        {
            string[] args = { handler };
            //Create command for closing handler.
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CloseHandler, args, "");
            //Send command.
            TCPClientChannel.GetTCPClientChannel().Send(command);
            //Remove handler from list.
            Handlers.Remove(handler);
        }

    }
}
