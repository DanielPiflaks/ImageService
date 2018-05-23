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
using Infrastructure.Event;
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

        public SettingsModel()
        {
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, null, "");
            TCPClientChannel.GetTCPClientChannel().NotifyMessage += UpdateByNotification;
            string settingsMsg = TCPClientChannel.GetTCPClientChannel().SendAndReceive(command);
            UpdateByNotification(settingsMsg);
        }

        public void UpdateByNotification(string message)
        {
            ConfigurationRecieveEventArgs configurationNotify =
                JsonConvert.DeserializeObject<ConfigurationRecieveEventArgs>(message);
            try
            {
                switch ((ConfigurationEnum)configurationNotify.ConfigurationID)
                {
                    case ConfigurationEnum.SettingsConfiguration:
                        SetSettings(configurationNotify.Args);
                        break;
                    case ConfigurationEnum.RemoveHandler:
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

        public void SetSettings(string[] settings)
        {
            Handlers = new ObservableCollection<string>();

            OutputDir = settings[0];
            ThumbnailSize = settings[1];
            LogName = settings[2];
            SourceName = settings[3];

            for (int i = 4; i < settings.Length; i++)
            {
                Handlers.Add(settings[i]);
            }
        }

        public void RemoveHandler(string handler)
        {
            string[] args = { handler };
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CloseHandler, args, "");
            TCPClientChannel.GetTCPClientChannel().Send(command);

            Handlers.Remove(handler);
        }

    }
}
