using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
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
        /// <summary>
        /// property changed function.
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<MessageRecievedEventArgs> LogMessages { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public LogModel()
        {
            LogMessages = new ObservableCollection<MessageRecievedEventArgs>();
            // Set new command for creating log.
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, "");
            // Adding notify function.
            TCPClientChannel.GetTCPClientChannel().NotifyMessage += UpdateByNotification;
            // Send command and recevie back log history.
            string settingsMsg = TCPClientChannel.GetTCPClientChannel().SendAndReceive(command);
            // Add log history to log.
            UpdateByNotification(settingsMsg);
        }

        /// <summary>
        /// Update log.
        /// </summary>
        /// <param name="message"> received message</param>
        public void UpdateByNotification(string message)
        {
            
            try
            {
                // Wrap given message in Json.
                ConfigurationRecieveEventArgs configurationNotify =
                      JsonConvert.DeserializeObject<ConfigurationRecieveEventArgs>(message);

                // Update log according to message type.
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

        /// <summary>
        /// Add message to log (LIFO).
        /// </summary>
        /// <param name="newMessage"> message to add.</param>
        public void AddLogMessage(string[] newMessage)
        {
            // Create message object.
            MessageRecievedEventArgs message =
                new MessageRecievedEventArgs((MessageTypeEnum)int.Parse(newMessage[0]), newMessage[1]);
            // Add it to log messages.
            Application.Current.Dispatcher.Invoke(new Action(() =>
            { LogMessages.Insert(0, message); }));
        }

        /// <summary>
        /// Set log history.
        /// </summary>
        /// <param name="history"> earlier log messages to add to log.</param>
        public void SetLogHistory(string[] history)
        {
            // For each message in log history.
            for (int i = 0; i < history.Length; i = i + 2)
            {
                // Add to log messages.
                LogMessages.Insert(0, new MessageRecievedEventArgs((MessageTypeEnum)int.Parse(history[i]), history[i + 1]));
            }
        }
    }
}
