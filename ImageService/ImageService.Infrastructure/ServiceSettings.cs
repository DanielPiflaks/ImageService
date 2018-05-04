using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ImageService
{
    public class ServiceSettings
    {
        #region Members
        private string[] m_handlers;
        private string m_outputDir;
        private string m_sourceName;
        private string m_logName;
        private int m_thumbnailSize;
        #endregion

        #region Properties
        public string[] Handlers
        {
            get
            {
                return m_handlers;
            }
            set
            {
                m_handlers = value;
            }
        }
        public string OutputDir
        {
            get
            {
                return m_outputDir;
            }
            set
            {
                m_outputDir = value;
            }
        }
        public string SourceName
        {
            get
            {
                return m_sourceName;
            }
            set
            {
                m_sourceName = value;
            }
        }
        public string LogName
        {
            get
            {
                return m_logName;
            }
            set
            {
                m_logName = value;
            }
        }
        public int ThumbnailSize
        {
            get
            {
                return m_thumbnailSize;
            }
            set
            {
                m_thumbnailSize = value;
            }
        }
        #endregion

        private static ServiceSettings serviceSettings;
        /// <summary>
        /// Constructor.
        /// </summary>
        private ServiceSettings()
        {
            //Split handlers by ;.
            Handlers = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
            OutputDir = ConfigurationManager.AppSettings.Get("OutputDir");
            SourceName = ConfigurationManager.AppSettings.Get("SourceName");
            LogName = ConfigurationManager.AppSettings.Get("LogName");
            m_thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings.Get("ThumbnailSize"));
        }

        /// <summary>
        /// Static function to get service settings(singleton).
        /// </summary>
        /// <returns></returns>
        public static ServiceSettings GetServiceSettings()
        {
            try
            {
                //Check if settings loaded.
                if (serviceSettings == null)
                {
                    //If not loaded, create service settings.
                    serviceSettings = new ServiceSettings();
                }
                //Return service settings.
                return serviceSettings;
            }
            catch (Exception e)
            {
                //If failed to create settings - throw exception.
                throw new Exception(e.Message);
            }

        }

    }
}
