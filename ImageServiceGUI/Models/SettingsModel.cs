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
using Infrastructure.Modal;

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

        private int m_thumbnailSize;
        public int ThumbnailSize
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

            string settingsMsg = TCPClientChannel.GetTCPClientChannel().SendAndReceive(command);
            SettingsParams settings = JsonConvert.DeserializeObject<SettingsParams>(settingsMsg);

            if (settings is SettingsParams)
            {
                SettingsParams settingsObj = (SettingsParams) settings;
                OutputDir = settingsObj.OutputDir;
                ThumbnailSize = settingsObj.ThumbnailSize;
                LogName = settingsObj.LogName;
                SourceName = settingsObj.SourceName;
            }
            else
            {
                throw new Exception("Error");
            }
        }
    }
}
