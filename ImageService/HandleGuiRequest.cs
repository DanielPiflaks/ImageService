using Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    public class HandleGuiRequest
    {
        public static void handle(object recievedObject, TCPServerChannel tcpServerChannel, Socket s)
        {
            if (recievedObject is CommandMessage)
            {
                CommandMessage msg = (CommandMessage)recievedObject;
                switch (msg.CmdEnum)
                {
                    case Infrastructure.Enums.CommandEnum.NewFileCommand:
                        break;
                    case Infrastructure.Enums.CommandEnum.GetConfigCommand:
                        //tcpServerChannel.SendMessage(MessageDecoder.Serialize(ServiceSettings.GetServiceSettings()), s);
                        break;
                    case Infrastructure.Enums.CommandEnum.LogCommand:
                        break;
                    case Infrastructure.Enums.CommandEnum.CloseCommand:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
