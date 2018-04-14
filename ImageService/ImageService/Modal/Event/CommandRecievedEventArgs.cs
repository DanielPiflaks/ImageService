using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class CommandRecievedEventArgs : EventArgs
    {
        // The Command ID
        public int CommandID { get; set; }      
        public string[] Args { get; set; }
        // The Request Directory
        public string RequestDirPath { get; set; }  

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">ID of command</param>
        /// <param name="args">Arguments for command</param>
        /// <param name="path">Path for requested directory.</param>
        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
