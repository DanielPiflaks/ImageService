using ImageService.Infrastructure;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="modal">Image service modal.</param>
        public NewFileCommand(IImageServiceModal modal)
        {
            // Storing the Modal
            m_modal = modal;
        }

        /// <summary>
        /// The Function That will Execute The command
        /// </summary>
        /// <param name="args"> Arguments to execute command.</param>
        /// <param name="result">Out for result of execution.</param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            //Check if there are enough arguments to add file.
            if (args.Length >= 1)
            {
                return m_modal.AddFile(args[0], out result);
            }
            else
            {
                //If not, set result to false and return corresponding message.
                result = false;
                return "There are no args for new file command to execute";
            }
        }
    }
}
