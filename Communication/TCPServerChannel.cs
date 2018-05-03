using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public class TCPServerChannel
    {
        #region Properties
        private int m_port;
        public int Port
        {
            get { return m_port; }
            set { m_port = value; }
        }

        private string m_ip;
        public string IP
        {
            get { return m_ip; }
            set { m_ip = value; }
        }

        #endregion
    }
}
