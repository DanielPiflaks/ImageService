using ImageService.Commands;
using Infrastructure;
using Infrastructure.Enums;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        // The Modal Object
        private IImageServiceModal m_modal;                      
        private Dictionary<int, ICommand> commands;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="modal">Image service modal.</param>
        public ImageController(IImageServiceModal modal)
        {
            // Storing the Modal Of The System
            m_modal = modal;                    
            commands = new Dictionary<int, ICommand>()
            {
                   // For Now will contain NEW_FILE_COMMAND
                { (int) CommandEnum.NewFileCommand,  new NewFileCommand(modal)}
            };
        }

        /// <summary>
        /// Executing the Command Requet
        /// </summary>
        /// <param name="commandID">Command ID</param>
        /// <param name="args">Arguments for command</param>
        /// <param name="result">Result of execution</param>
        /// <returns></returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            //Check if wanted command ID is exist.
            if (commands.ContainsKey(commandID))
            {
                //Create new task to handle execution of command.
                Task<Tuple<string, bool>> task = new Task<Tuple<string, bool>>(() =>
                {
                    bool result;
                    //Execute command.
                    string resultMessage = commands[commandID].Execute(args, out result);
                    //Create tuple with result of execution.
                    Tuple<string, bool> output = Tuple.Create(resultMessage, result);
                    return output;
                });
                //Start task.
                task.Start();
                //Wait until task finish.
                task.Wait();
                //Update resultSuccesful with result of task.
                resultSuccesful = task.Result.Item2;
                //Return result message.
                return task.Result.Item1;
            }
            else
            {
                //If wanted command is not exist in dictionary then notify caller.
                resultSuccesful = false;
                return "Command does not exist in dictionary.";
            }
        }
    }
}
