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
        public SettingViewModel SettingViewModel { get; set; }
        public LogViewModel LogViewModel { get; set; }
        public MainWindowModel MainWindowModel { get; set; }
        public IsConnectedEnum VMIsConnected
        {
            get { return MainWindowModel.IsConnected; }
        }

        public MainWindowViewModel()
        {
            this.MainWindowModel = new MainWindowModel();
            if (VMIsConnected == IsConnectedEnum.Connected)
            {
                this.SettingViewModel = new SettingViewModel();
                this.LogViewModel = new LogViewModel();
                TCPClientChannel.GetTCPClientChannel().ListenToServer();
            }
        }



    }
}
