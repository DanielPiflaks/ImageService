using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Interfaces
{
    public interface IHandleClient
    {
          void handle(TCPServerChannel tcpServerChannel, TcpClient client);
    }
}
