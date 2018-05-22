using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Event
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ConfigurationRecieveEventArgs : EventArgs
    {
        // The Configuration ID
        [JsonProperty(PropertyName = "ConfigurationID")]
        public int ConfigurationID { get; set; }
        [JsonProperty(PropertyName = "Args")]
        public string[] Args { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">ID of command</param>
        /// <param name="args">Arguments for command</param>
        [JsonConstructor]
        public ConfigurationRecieveEventArgs(int id, string[] args)
        {
            ConfigurationID = id;
            Args = args;
        }
    }
}
