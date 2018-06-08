using Communication;
using ImageService.Infrastructure.Enums;
using Infrastructure.Enums;
using Infrastructure.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ConfigModel
    {
        #region Properties

        [Required]
        [Display(Name = "Handlers")]
        public List<string> Handlers { get; set; }

        [Required]
        [Display(Name = "OutputDir")]
        public string OutputDir
        {
            get; set;
        }

        [Required]
        [Display(Name = "SourceName")]
        public string SourceName
        {
            get; set;
        }

        [Required]
        [Display(Name = "LogName")]
        public string LogName
        {
            get; set;
        }

        [Required]
        [Display(Name = "ThumbnailSize")]
        public string ThumbnailSize
        {

            get; set;
        }

        #endregion
        public ConfigModel()
        {
            //Create command.
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, null, "");

            try
            {
                //Add function to event.
                //TCPClientChannel.GetTCPClientChannel().NotifyMessage += UpdateByNotification;
                TCPClientChannel.GetTCPClientChannel().DisconnectClientChannel();
                //Send command.
                string settingsMsg = TCPClientChannel.GetTCPClientChannel().SendAndReceive(command);
                //Update notification.
                UpdateByNotification(settingsMsg);
                //TCPClientChannel.GetTCPClientChannel().ListenToServer();
            }
            catch (Exception)
            {
                string[] emptyArgs = { " ", " ", " ", " " };
                SetSettings(emptyArgs);
            }
        }

        /// <summary>
        /// Update settings model by given message.
        /// </summary>
        /// <param name="message"></param>
        public bool UpdateByNotification(string message)
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
                            /*Application.Current.Dispatcher.Invoke(new Action(() =>
                            { Handlers.Remove(configurationNotify.Args[0]); }));*/
                            Handlers.Remove(configurationNotify.Args[0]);
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
            return true;
        }

        /// <summary>
        /// Sets settings.
        /// </summary>
        /// <param name="settings">Given settings.</param>
        public void SetSettings(string[] settings)
        {
            Handlers = new List<string>();
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
        public HandlerRemoval RemoveHandler(string handler)
        {
            string[] args = { handler };
            //Create command for closing handler.
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CloseHandler, args, "");
            //Send command.
            string returnMessage = TCPClientChannel.GetTCPClientChannel().SendAndReceive(command);
            if (UpdateByNotification(returnMessage))
            {
                Thread.Sleep(200);
                if (Handlers.Contains(handler))
                {
                    return HandlerRemoval.FailedToRemove;
                }
                else
                {
                    return HandlerRemoval.Removed;
                }
            }
            return HandlerRemoval.FailedToRemove;
        }
    }
}