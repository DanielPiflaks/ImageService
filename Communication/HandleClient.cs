using ImageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    class HandleClient
    {
        public static object GetClientRequest(object recievedObject)
        {
            //Message returnVal = null;
            if (recievedObject is CommandMessage)
            {
                CommandMessage msg = (CommandMessage)recievedObject;
                switch (msg.CmdEnum)
                {
                    case ImageService.Infrastructure.Enums.CommandEnum.NewFileCommand:
                        break;
                    case ImageService.Infrastructure.Enums.CommandEnum.GetConfigCommand:
                        //returnVal = MessageDecoder.Serialize(ServiceSettings.GetServiceSettings());
                        break;
                    case ImageService.Infrastructure.Enums.CommandEnum.LogCommand:
                        break;
                    case ImageService.Infrastructure.Enums.CommandEnum.CloseCommand:
                        break;
                    default:
                        break;
                }
            }
            return null;
        }
    }
}
