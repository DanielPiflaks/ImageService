using ImageServiceGUI.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public string VMOutputDir
        {
            get { return SettingsModel.OutputDir; }
        }

        public string VMSourceName
        {
            get { return SettingsModel.SourceName; }
        }

        public string VMLogName
        {
            get { return SettingsModel.LogName; }
        }

        public string VMThumbnailSize
        {
            get { return SettingsModel.ThumbnailSize; }
        }

        public ObservableCollection<string> VMHandlers
        {
            get {return SettingsModel.Handlers; }
        }


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
        /// <summary>
        /// Constructor.
        /// </summary>
        public SettingViewModel()
        {
            //Create settings model.
            SettingsModel = new SettingsModel();
            SelectedHandler = null;
            //Set property change.
            SettingsModel.PropertyChanged +=
              delegate (Object sender, PropertyChangedEventArgs e)
              {
                  OnPropertyChanged(e.PropertyName);
              };
            //Create delegate command.
            RemoveCommand = new DelegateCommand<object>(RemoveSelectedHandler, CanRemove);
        }

        /// <summary>
        /// Remove selected handler from model.
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveSelectedHandler(object obj)
        {
            SettingsModel.RemoveHandler(SelectedHandler);
        }
        /// <summary>
        /// Function that check if can press on button to remove selected handler.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CanRemove(object obj)
        {
            //Check if seleceted handler is not null
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
