using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public interface ICommand
    {
        /// <summary>
        /// The Function That will Execute The command
        /// </summary>
        /// <param name="args"> Arguments to execute command.</param>
        /// <param name="result">Out for result of execution.</param>
        /// <returns></returns>
        string Execute(string[] args, out bool result);      
    }
}
