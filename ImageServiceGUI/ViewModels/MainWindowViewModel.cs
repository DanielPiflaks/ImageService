using Communication;
using ImageService.Infrastructure.Enums;
using ImageServiceGUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModels
{
    class MainWindowViewModel
    {
        #region Properties
        public SettingViewModel SettingViewModel { get; set; }
        public LogViewModel LogViewModel { get; set; }
        public MainWindowModel MainWindowModel { get; set; }
        public IsConnectedEnum VMIsConnected
        {
            get { return MainWindowModel.IsConnected; }
        }
        #endregion
        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindowViewModel()
        {
            //Create main window model.
            this.MainWindowModel = new MainWindowModel();
            //Check if there is connection.
            if (VMIsConnected == IsConnectedEnum.Connected)
            {
                //Create constructor for models.
                this.SettingViewModel = new SettingViewModel();
                this.LogViewModel = new LogViewModel();
                //Listen to server.
                TCPClientChannel.GetTCPClientChannel().ListenToServer();
            }
        }



    }
}
