using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Set ip to be local host.
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1102);

            //Create tcp listener.
            TcpListener Listener = new TcpListener(ep);
            //Start to listen.
            Listener.Start();
            TcpClient client = Listener.AcceptTcpClient();

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

                    string fileName = System.Text.Encoding.Default.GetString(fileNameBytes);

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
                    //saveImageInHandler(inbuffer);
                    // convert the stream of bytes to an image
                    Image img = (Bitmap)((new ImageConverter()).ConvertFrom(imageBytes));
                    img.Save(@"C:\" + fileName);

                } while (stream.DataAvailable);
            }

            //Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static int ByteArrayToInt(byte[] byteArray)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(byteArray);

            int size = BitConverter.ToInt32(byteArray, 0);
            return size;
        }

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
    }
}
