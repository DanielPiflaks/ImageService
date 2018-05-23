using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Event;

namespace Communication.Interfaces
{
    interface ITCPClientChannel
    {
        void Send(CommandRecievedEventArgs command);
        string Receive();
        string SendAndReceive(CommandRecievedEventArgs command);
        void ListenToServer();
    }
}
