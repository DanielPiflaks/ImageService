using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceGUI.Models;
using System.Collections.ObjectModel;
using ImageService.Logging.Modal;

namespace ImageServiceGUI.ViewModels
{
    public class LogViewModel : INotifyPropertyChanged
    {
        #region Properties
        public ObservableCollection<MessageRecievedEventArgs> LogMessages
        {
            get { return LogModel.LogMessages; }
        }

        private LogModel logView;
        public LogModel LogModel
        {
            get { return this.logView; }
            set { this.logView = value; }
        }
        #endregion
        /// <summary>
        /// Constructor.
        /// </summary>
        public LogViewModel()
        {
            //Create model.
            LogModel = new LogModel();
            //Set property changed.
            LogModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                OnPropertyChanged(e.PropertyName);
            };
        }

        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String a)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(a));
            }
        }
        #endregion
    }
}
