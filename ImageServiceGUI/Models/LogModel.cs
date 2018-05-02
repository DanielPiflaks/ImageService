using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ImageServiceGUI.Models
{
    public class LogModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private string m_type;
        public string Type
        {
            get { return m_type; }
            set
            {
                m_type = value;
                OnPropertyChanged("Type");
            }
        }

        private string m_msg;
        public string Message
        {
            get { return m_msg; }
            set
            {
                m_type = value;
                OnPropertyChanged("Msg");
            }
        }
    }
}
