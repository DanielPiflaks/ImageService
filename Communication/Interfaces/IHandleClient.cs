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
          /// <summary>
          /// Handles client.
          /// </summary>
          /// <param name="client">Client to handle.</param>
          void handle(TcpClient client);
    }
}
