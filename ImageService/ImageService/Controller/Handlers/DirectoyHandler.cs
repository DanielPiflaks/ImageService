using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;
using ImageService.Commands;
using ImageService.Server;

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

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        public DirectoyHandler(IImageController imageController, ILoggingService loggingService,
            string path)
        {
            m_controller = imageController;
            m_logging = loggingService;
            m_path = path;
            try
            {
                m_dirWatcher = new FileSystemWatcher(this.m_path);
            }
            catch (Exception)
            {
                m_logging.Log("Failed build directory watcher for: " + m_path, MessageTypeEnum.FAIL);
            }
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

            m_dirWatcher.Created += new FileSystemEventHandler(NewFileHandler);
            m_dirWatcher.Changed += new FileSystemEventHandler(NewFileHandler);
            m_dirWatcher.EnableRaisingEvents = true;
            try
            {
                string[] files = Directory.GetFiles(dirPath);
                foreach (var file in files)
                {
                    try
                    {
                        string fileExtension = Path.GetExtension(file);
                        if (m_filesExtention.FindIndex(x => x.Equals(fileExtension, StringComparison.OrdinalIgnoreCase)) != -1)
                        {
                            string[] args = { file };
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
            string output = m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
            MessageTypeEnum messageType;
            if (result)
            {
                messageType = MessageTypeEnum.INFO;
            }
            else
            {
                messageType = MessageTypeEnum.FAIL;
            }
            m_logging.Log(output, messageType);
        }


        public void NewFileHandler(object source, FileSystemEventArgs e)
        {
            try
            {
                string file = e.FullPath;
                string fileExtension = Path.GetExtension(file);

                if (m_filesExtention.Contains(fileExtension))
                {
                    string[] args = { file };
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

        public void CloseHandler(object sender, DirectoryCloseEventArgs e)
        {
            try
            {
                m_dirWatcher.EnableRaisingEvents = false;
                ((ImageServer)sender).CommandRecieved -= this.OnCommandRecieved;
                m_logging.Log("Close handler of " + m_path, MessageTypeEnum.INFO);
            }
            catch
            {
                m_logging.Log("Failed closing handler of " + m_path, MessageTypeEnum.FAIL);
            }
        }
    }
}
