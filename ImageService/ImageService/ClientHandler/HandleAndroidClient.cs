using Communication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using ImageService.Commands;
using ImageService.Logging;
using System.Drawing;
using System.IO;
using ImageService.Modal;
using Infrastructure.Enums;
using ImageService.Infrastructure.Enums;
using System.Threading;

namespace ImageService.ClientHandler
{
    class HandleAndroidClient : IHandleClient
    {
        private Dictionary<int, ICommand> commands;
        private ILoggingService m_logging;
        private string m_directoryHandler;
        private bool m_stopTask;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logging">logging</param>
        /// <param name="imageServer">Image server.</param>
        public HandleAndroidClient(ILoggingService logging, IImageServiceModal modal, string directoryHandler)
        {
            m_directoryHandler = directoryHandler;
            //Create dictionary from enum int to ICommand
            commands = new Dictionary<int, ICommand>()
            {
                { (int) CommandEnum.NewFileCommand,  new NewFileCommand(modal)}
            };
            m_logging = logging;
            m_stopTask = false;
        }

        public void handle(TcpClient client)
        {
            //Create task for handle client.
            Task task = new Task(() =>
            {
                try
                {
                    //While client is working.
                    while (!m_stopTask)
                    {
                        NetworkStream stream = client.GetStream();

                        //Get reader from stream.
                        BinaryReader reader = new BinaryReader(stream);

                        MemoryStream messageStream = new MemoryStream();
                        byte[] messageSizeBytes = new byte[4];
                        int messageSize, counter;

                        if (stream.CanRead)
                        {
                            Thread.Sleep(2000);
                            do
                            {
                                int numberOfBytesRead = stream.Read(messageSizeBytes, 0, messageSizeBytes.Length);
                                messageSize = ByteArrayToInt(messageSizeBytes);
                                counter = 0;
                                byte[] fileNameBytes = new byte[messageSize];
                                while (counter < messageSize)
                                {
                                    if (stream.DataAvailable)
                                    {
                                        counter += stream.Read(fileNameBytes, counter, messageSize - counter);
                                    }
                                }

                                string fileName = Encoding.Default.GetString(fileNameBytes);

                                numberOfBytesRead = stream.Read(messageSizeBytes, 0, messageSizeBytes.Length);
                                messageSize = ByteArrayToInt(messageSizeBytes);
                                counter = 0;
                                byte[] imageBytes = new byte[messageSize];
                                while (counter < messageSize)
                                {
                                    if (stream.DataAvailable)
                                    {
                                        counter += stream.Read(imageBytes, counter, messageSize - counter);
                                    }
                                }
                                // convert the stream of bytes to an image
                                Image img = (Bitmap)((new ImageConverter()).ConvertFrom(imageBytes));
                                img.Save(m_directoryHandler + @"\" + fileName);
                            } while (stream.DataAvailable);
                        }
                    }
                }
                catch (Exception e)
                {
                    m_logging.Log("Problem in handleing client - " + e.Message, MessageTypeEnum.FAIL);
                }

            });
            //Start task.
            task.Start();

        }

        public static int ByteArrayToInt(byte[] byteArray)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(byteArray);

            int size = BitConverter.ToInt32(byteArray, 0);
            return size;
        }
    }
}
