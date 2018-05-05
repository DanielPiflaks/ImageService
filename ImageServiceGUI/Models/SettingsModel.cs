using ImageService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;
using Newtonsoft.Json;

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
            string settingsMsg = TCPClientChannel.GetTCPClientChannel().SendAndReceive(ImageService.Infrastructure.Enums.CommandEnum.GetConfigCommand, null);
            ServiceSettings settings = JsonConvert.DeserializeObject<ServiceSettings>(settingsMsg);

            if (settings is ServiceSettings)
            {
                ServiceSettings settingsObj = (ServiceSettings) settings;
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
