using Newtonsoft.Json;
using System;

namespace Infrastructure.Event
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CommandRecievedEventArgs : EventArgs
    {
        // The Command ID
        [JsonProperty(PropertyName = "CommandID")]
        public int CommandID { get; set; }
        [JsonProperty(PropertyName = "Args")]
        public string[] Args { get; set; }
        // The Request Directory
        [JsonProperty(PropertyName = "RequestDirPath")]
        public string RequestDirPath { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">ID of command</param>
        /// <param name="args">Arguments for command</param>
        /// <param name="path">Path for requested directory.</param>
        [JsonConstructor]
        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
