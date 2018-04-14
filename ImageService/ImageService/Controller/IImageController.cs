using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public interface IImageController
    {
        /// <summary>
        /// Executing the Command Requet
        /// </summary>
        /// <param name="commandID">Command ID</param>
        /// <param name="args">Arguments for command</param>
        /// <param name="result">Result of execution</param>
        /// <returns></returns>
        string ExecuteCommand(int commandID, string[] args, out bool result);
    }
}
