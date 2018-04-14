using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    public interface IDirectoryHandler
    {
        //The Event That Notifies that the Directory is being closed
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        /// <summary>
        /// The Function Recieves the directory to Handle
        /// </summary>
        /// <param name="dirPath">Directory path to handle.</param>
        void StartHandleDirectory(string dirPath);
        /// <summary>
        /// The Event that will be activated upon new Command
        /// </summary>
        /// <param name="sender">Who asked for command.</param>
        /// <param name="e">Comand recieved event arguments.</param>
        void OnCommandRecieved(object sender, CommandRecievedEventArgs e); 
    }
}
