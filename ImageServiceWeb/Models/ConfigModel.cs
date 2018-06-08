using Communication;
using ImageService.Infrastructure.Enums;
using Infrastructure.Enums;
using Infrastructure.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ConfigModel
    {
        #region Properties

        [Required]
        [Display(Name = "Handlers")]
        public static List<string> Handlers { get; set; }

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
            //Add function to event.
            TCPClientChannel.GetTCPClientChannel().NotifyMessage += UpdateByNotification;

            try
            {
                //Send command.
                string settingsMsg = TCPClientChannel.GetTCPClientChannel().SendAndReceive(command);
                //Update notification.
                UpdateByNotification(settingsMsg);
            }
            catch (Exception)
            {
                string[] emptyArgs= {" ", " ", " ", " "};
                SetSettings(emptyArgs);
            }
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
    }
}