using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ImageService.Logging.Modal;
using Infrastructure.Event;
using Infrastructure.Enums;
using Communication;
using Newtonsoft.Json;
using ImageService.Infrastructure.Enums;
using System.Windows;

namespace ImageServiceGUI.Models
{
    public class LogModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<MessageRecievedEventArgs> LogMessages { get; set; }


        public LogModel()
        {
            LogMessages = new ObservableCollection<MessageRecievedEventArgs>();

            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, "");
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
                    case ConfigurationEnum.LogHistory:
                        SetLogHistory(configurationNotify.Args);
                        break;
                    case ConfigurationEnum.NewLogMessageConfiguraton:
                        AddLogMessage(configurationNotify.Args);
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

        public void AddLogMessage(string[] newMessage)
        {
            MessageRecievedEventArgs message =
                new MessageRecievedEventArgs((MessageTypeEnum)int.Parse(newMessage[0]), newMessage[1]);
            Application.Current.Dispatcher.Invoke(new Action(() =>
            { LogMessages.Insert(0, message); }));
        }

        public void SetLogHistory(string[] history)
        {
            for (int i = 0; i < history.Length; i = i + 2)
            {
                LogMessages.Insert(0, new MessageRecievedEventArgs((MessageTypeEnum)int.Parse(history[i]), history[i + 1]));
            }
        }
    }
}
