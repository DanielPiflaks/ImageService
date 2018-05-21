using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService
{
    public class HandlerListManager
    {
        private string[] m_handlers;
        private static Mutex mutex = new Mutex();

        public string[] Handlers
        {
            get { return m_handlers; }
            set { m_handlers = value; }
        }

        private static HandlerListManager handlerListManger;

        private HandlerListManager()
        {
            Handlers = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
        }

        public void removeHandler(string handler)
        {
            mutex.WaitOne();
            Handlers = Handlers.Where(val => val != handler).ToArray();
            mutex.ReleaseMutex();
        }

        public static HandlerListManager GetHandlerListManager()
        {
            try
            {
                mutex.WaitOne();
                if (handlerListManger == null)
                {
                    handlerListManger = new HandlerListManager();
                }
                mutex.ReleaseMutex();
                return handlerListManger;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
