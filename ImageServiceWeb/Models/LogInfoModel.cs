using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Communication;
using ImageService.Infrastructure.Enums;
using Infrastructure.Enums;
using Infrastructure.Event;
using Newtonsoft.Json;

namespace ImageServiceWeb.Models
{
    public class LogInfoModel
    {
        #region Properties
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Messages")]
        public List<Log> LogMessages { get; set; }
        public delegate void NotifyChange();
        public event NotifyChange notify;
        #endregion

        public LogInfoModel()
        {
            LogMessages = new List<Log>();
            try
            {
                // Set new command for creating log.
                CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, "");
                // Send command and recevie back log history.
                TCPClientChannel.GetTCPClientChannel().DisconnectClientChannel();
                string settingsMsg = TCPClientChannel.GetTCPClientChannel().SendAndReceive(command);
                // Adding notify function.
                TCPClientChannel.GetTCPClientChannel().NotifyMessage += UpdateByNotification;
                // Add log history to log.
                UpdateByNotification(settingsMsg);
                TCPClientChannel.GetTCPClientChannel().ListenToServer();
            }
            catch (Exception)
            {

            }
        }

        public List<T> GetEnumList<T>()
        {
            T[] array = (T[])Enum.GetValues(typeof(T));
            List<T> list = new List<T>(array);
            return list;
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
                notify?.Invoke();

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
            Log message =
                new Log((MessageTypeEnum)int.Parse(newMessage[0]), newMessage[1]);
            // Add it to log messages.
            //Application.Current.Dispatcher.Invoke(new Action(() =>
            //{ LogMessages.Insert(0, message); }));
            LogMessages.Insert(0, message);
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
                LogMessages.Insert(0, new Log((MessageTypeEnum)int.Parse(history[i]), history[i + 1]));
            }
        }
    }

}
