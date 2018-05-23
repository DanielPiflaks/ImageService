using Communication;
using ImageService.Infrastructure.Enums;
using Infrastructure.Enums;
using Infrastructure.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Models
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private IsConnectedEnum m_isConnected;
        public IsConnectedEnum IsConnected
        {
            get { return m_isConnected; }
            set
            {
                m_isConnected = value;
                OnPropertyChanged("OutputDir");
            }
        }

        public MainWindowModel()
        {
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.EchoCommand, null, "");
            try
            {
                string message = TCPClientChannel.GetTCPClientChannel().SendAndReceive(command);
                ConfigurationRecieveEventArgs returnParam =
                     JsonConvert.DeserializeObject<ConfigurationRecieveEventArgs>(message);
                if ((ConfigurationEnum)returnParam.ConfigurationID == ConfigurationEnum.Ack)
                {
                    IsConnected = IsConnectedEnum.Connected;
                }
            }
            catch (Exception)
            {
                IsConnected = IsConnectedEnum.NotConnected;
            }           
        }
    }
}
