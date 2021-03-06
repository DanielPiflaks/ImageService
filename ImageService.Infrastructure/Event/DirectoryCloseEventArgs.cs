﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Event
{
    public class DirectoryCloseEventArgs : EventArgs
    {
        public string DirectoryPath { get; set; }

        // The Message That goes to the logger
        public string Message { get; set; }             

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dirPath">Directory path.</param>
        /// <param name="message">Message when closing.</param>
        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            // Setting the Directory Name
            DirectoryPath = dirPath;
            // Storing the String          
            Message = message;                          
        }

    }
}
