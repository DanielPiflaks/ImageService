using ImageServiceGUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModels
{
    class SettingViewModel : INotifyPropertyChanged
    {
        private SettingsModel m_settingsModel;
        public SettingsModel SettingsModel
        {
            get { return this.m_settingsModel; }
            set
            {
                this.m_settingsModel = value;
            }
        }

        
        public SettingViewModel()
        {
            this.SettingsModel = new SettingsModel();
            SettingsModel.PropertyChanged +=
              delegate (Object sender, PropertyChangedEventArgs e)
              {
                  OnPropertyChanged(e.PropertyName);
              };
        }

        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
