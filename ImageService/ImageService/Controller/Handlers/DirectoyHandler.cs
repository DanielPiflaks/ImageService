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

namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private readonly string[] m_filesExtention = { "*.jpg", "*.png", "*.gif", "*.bmp" };
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
            m_dirWatcher = new FileSystemWatcher(path);
        }

        public void StartHandleDirectory(string dirPath)
        {
            m_dirWatcher.Created += new FileSystemEventHandler(NewFileHandler);
            m_dirWatcher.Changed += new FileSystemEventHandler(NewFileHandler);
            m_dirWatcher.EnableRaisingEvents = true;

            string[] files = Directory.GetFiles(dirPath);

            foreach (var file in files)
            {
                string fileExtension = Path.GetExtension(file);
                if (m_filesExtention.Contains(fileExtension))
                {
                    string[] args = { file };
                    CommandRecievedEventArgs newCommand = new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand,
                        args, file);
                    OnCommandRecieved(this, newCommand);
                }
            }
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool result;
            string output = m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
        }


        public void NewFileHandler(object source, FileSystemEventArgs e)
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
        }
    }
}
