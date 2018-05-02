using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModels
{
    class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            this.SettingViewModel = new SettingViewModel();
        }

        public SettingViewModel SettingViewModel { get; set; }
    }
}
