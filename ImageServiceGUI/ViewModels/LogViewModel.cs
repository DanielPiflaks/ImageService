using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceGUI.Models;

namespace ImageServiceGUI.ViewModels
{
    public class LogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String a)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(a));
            }
        }

        private LogModel logView;
        public LogModel LogModel
        {
            get { return this.logView; }
            set { this.logView = value; }
        }

        public LogViewModel()
        {
            this.LogModel = new LogModel();
            LogModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs type)
            {
                NotifyPropertyChanged(type.PropertyName);
            };
        }
        
    }
}
