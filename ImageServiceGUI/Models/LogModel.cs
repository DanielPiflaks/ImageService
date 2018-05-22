using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ImageService.Logging.Modal;

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

        public ObservableCollection<MessageRecievedEventArgs> LogMessages { get; set; }

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

        public LogModel()
        {
            LogMessages = new ObservableCollection<MessageRecievedEventArgs>();

            LogMessages.Add(new MessageRecievedEventArgs(MessageTypeEnum.INFO, "Working"));
            LogMessages.Add(new MessageRecievedEventArgs(MessageTypeEnum.WARNING, "Also Working"));
        }
    }
}
