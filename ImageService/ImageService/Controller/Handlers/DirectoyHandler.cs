using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using ImageService.Logging;
using System.Text.RegularExpressions;
using ImageService.Commands;
using ImageService.Server;
using Infrastructure.Event;
using Infrastructure.Enums;
using ImageService.Infrastructure.Enums;

namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private readonly List<string> m_filesExtention;
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        #endregion
        // The Event That Notifies that the Directory is being closed
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="imageController">Image controller.</param>
        /// <param name="loggingService">Logging modal</param>
        /// <param name="path">Path.</param>
        public DirectoyHandler(IImageController imageController, ILoggingService loggingService,
            string path)
        {
            m_controller = imageController;
            m_logging = loggingService;
            m_path = path;
            try
            {
                //Set file systerm watcher.
                m_dirWatcher = new FileSystemWatcher(this.m_path);
            }
            catch (Exception)
            {
                //Notify log.
                m_logging.Log("Failed create directory watcher for: " + m_path, MessageTypeEnum.FAIL);
            }
            //Create list of extention
            m_filesExtention = new List<string>
            {
                ".jpg", ".png", ".gif", ".bmp"
            };
        }

        /// <summary>
        /// The Function Recieves the directory to Handle
        /// </summary>
        /// <param name="dirPath">Directory path to handle.</param>
        public void StartHandleDirectory(string dirPath)
        {
            m_path = dirPath;
            //Add watcher to directory.
            m_dirWatcher.Created += new FileSystemEventHandler(NewFileHandler);
            m_dirWatcher.Changed += new FileSystemEventHandler(NewFileHandler);

            m_dirWatcher.EnableRaisingEvents = true;
            try
            {
                //Get files in directory.
                string[] files = Directory.GetFiles(dirPath);
                foreach (var file in files)
                {
                    try
                    {
                        //Extract file extension.
                        string fileExtension = Path.GetExtension(file);
                        //Check if file has valid extension, case insesetive.
                        if (m_filesExtention.FindIndex(x => x.Equals(fileExtension, StringComparison.OrdinalIgnoreCase)) != -1)
                        {
                            string[] args = { file };
                            //Set new command.
                            CommandRecievedEventArgs newCommand = new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand,
                                args, file);
                            OnCommandRecieved(this, newCommand);
                            m_logging.Log("Start handle file: " + file, MessageTypeEnum.INFO);
                        }
                    }
                    catch (Exception)
                    {
                        m_logging.Log("Failed to start handler file: " + file, MessageTypeEnum.FAIL);
                    }
                }
                m_logging.Log("Start handle directory: " + dirPath, MessageTypeEnum.INFO);
            }
            catch (Exception)
            {

                m_logging.Log("Failed to start handler directory: " + dirPath, MessageTypeEnum.FAIL);
            }
        }

        /// <summary>
        /// The Event that will be activated upon new Command
        /// </summary>
        /// <param name="sender">Who asked for command.</param>
        /// <param name="e">Comand recieved event arguments.</param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool result;
            //Execute command by controller.
            string output = m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
            MessageTypeEnum messageType;
            //Check what kind of result- failure or success
            if (result)
            {
                messageType = MessageTypeEnum.INFO;
            }
            else
            {
                messageType = MessageTypeEnum.FAIL;
            }
            //Write to log according to result.
            m_logging.Log(output, messageType);
        }

        /// <summary>
        /// Create new file handler
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void NewFileHandler(object source, FileSystemEventArgs e)
        {
            try
            {
                string file = e.FullPath;
                //Extract file extensions.
                string fileExtension = Path.GetExtension(file);
                //Check if file has valid extension, case insesetive.
                if (m_filesExtention.FindIndex(x => x.Equals(fileExtension, StringComparison.OrdinalIgnoreCase)) != -1)
                {
                    string[] args = { file };
                    //Set new command.
                    CommandRecievedEventArgs newCommand = new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand,
                        args, file);
                    OnCommandRecieved(this, newCommand);
                }
                m_logging.Log("Create new file handler for: " + e.FullPath, MessageTypeEnum.INFO);
            }
            catch (Exception)
            {
                m_logging.Log("Failed create new file handler for: " + e.FullPath, MessageTypeEnum.FAIL);
            }

        }

        /// <summary>
        /// Close handlers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseHandler(object sender, DirectoryCloseEventArgs e)
        {
            try
            {
                //Disable raising events.
                m_dirWatcher.EnableRaisingEvents = false;
                //Remove event on command receive.
                ((ImageServer)sender).CommandRecieved -= this.OnCommandRecieved;
                m_dirWatcher.Created -= new FileSystemEventHandler(NewFileHandler);
                m_dirWatcher.Changed -= new FileSystemEventHandler(NewFileHandler);
                HandlerListManager.GetHandlerListManager().removeHandler(m_path);
                m_logging.Log("Close handler of " + m_path, MessageTypeEnum.INFO);
            }
            catch
            {
                m_logging.Log("Failed closing handler of " + m_path, MessageTypeEnum.FAIL);
            }
        }

        public string GetHandlerPath()
        {
            return this.m_path;
        }
    }
}
