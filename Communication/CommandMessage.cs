using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Communication
{
    [Serializable]
    public class CommandMessage : ISerializable
    {
        private CommandEnum m_cmdEnum;

        public CommandEnum CmdEnum
        {
            get { return m_cmdEnum; }
            set { m_cmdEnum = value; }
        }

        private List<String> m_args;

        public List<String> ArgsList
        {
            get { return m_args; }
            set { m_args = value; }
        }

        public CommandMessage(CommandEnum commandEnum, List<String> args)
        {
            m_cmdEnum = commandEnum;
            m_args = args;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentException("info");
            }

            info.AddValue("CmdEnum", m_cmdEnum);
            info.AddValue("ArgsList", m_args);           
        }
    }
}
