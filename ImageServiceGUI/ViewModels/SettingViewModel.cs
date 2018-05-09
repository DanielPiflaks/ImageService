using ImageServiceGUI.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageServiceGUI.ViewModels
{
    class SettingViewModel : INotifyPropertyChanged
    {
        #region Properties
        private SettingsModel m_settingsModel;
        public SettingsModel SettingsModel
        {
            get { return this.m_settingsModel; }
            set
            {
                this.m_settingsModel = value;
            }
        }

        private string m_selectedHandler;
        public string SelectedHandler
        {
            get
            {
                return m_selectedHandler;
            }
            set
            {
                m_selectedHandler = value;
                OnPropertyChanged("SelectedHandler");
                var command = this.RemoveCommand as DelegateCommand<object>;
                if (command != null)
                {
                    command.RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand RemoveCommand { get; private set; }
        #endregion

        public SettingViewModel()
        {
            SettingsModel = new SettingsModel();
            SelectedHandler = null;
            SettingsModel.PropertyChanged +=
              delegate (Object sender, PropertyChangedEventArgs e)
              {
                  OnPropertyChanged(e.PropertyName);
              };
            RemoveCommand = new DelegateCommand<object>(RemoveSelectedHandler, CanRemove);
        }


        public void RemoveSelectedHandler(object obj)
        {
            SettingsModel.RemoveHandler(SelectedHandler);
        }

        public bool CanRemove(object obj)
        {
            if (SelectedHandler != null)
            {
                return true;
            }
            else
            {
                return false;
            }
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
