using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Event;

namespace Communication.Interfaces
{
    interface ITCPServerChannel
    {
        void Start();
        void NotifyClientsOnChange(object sender, ConfigurationRecieveEventArgs e);

    }
}
