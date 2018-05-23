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
        private static Mutex mutex = new Mutex();
    
        private string[] m_handlers;
        public string[] Handlers
        {
            get { return m_handlers; }
            set { m_handlers = value; }
        }

        private static HandlerListManager handlerListManger;
        /// <summary>
        /// Constructor.
        /// </summary>
        private HandlerListManager()
        {
            //Get all handlers.
            Handlers = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
        }
        /// <summary>
        /// Removes handler.
        /// </summary>
        /// <param name="handler"></param>
        public void removeHandler(string handler)
        {
            mutex.WaitOne();
            Handlers = Handlers.Where(val => val != handler).ToArray();
            mutex.ReleaseMutex();
        }
        /// <summary>
        /// Returns handler list manager(singleton).
        /// </summary>
        /// <returns></returns>
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
