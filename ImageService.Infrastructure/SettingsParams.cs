using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SettingsParams
    {
        #region Members
        private string[] m_handlers;
        private string m_outputDir;
        private string m_sourceName;
        private string m_logName;
        private int m_thumbnailSize;
        #endregion

        #region Properties
        [JsonProperty(PropertyName = "Handlers")]
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
        [JsonProperty(PropertyName = "OutputDir")]
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
        [JsonProperty(PropertyName = "SourceName")]
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
        [JsonProperty(PropertyName = "LogName")]
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
        [JsonProperty(PropertyName = "ThumbnailSize")]
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

        [JsonConstructor]
        public SettingsParams(string[] handlers, string outputDir, string sourceName,
            string logName, int thumbnailSize)
        {
            Handlers = handlers;
            OutputDir = outputDir;
            SourceName = sourceName;
            LogName = logName;
            m_thumbnailSize = thumbnailSize;
        }
    }
}
