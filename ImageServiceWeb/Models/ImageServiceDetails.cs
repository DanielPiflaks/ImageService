using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Communication;
using Infrastructure.Event;
using Newtonsoft.Json;
using ImageService.Infrastructure.Enums;
using Infrastructure.Enums;
using System.IO;

namespace ImageServiceWeb.Models
{
    public class ImageServiceDetails
    {
        #region Properties
        private readonly List<string> m_filesExtention;

        private string m_outputDirPath;
        public string OutputDirPath
        {
            get { return m_outputDirPath; }
            set { m_outputDirPath = value; }
        }

        [Required]
        [Display(Name = "NumberOfPictures")]
        public int NumberOfPictures { get; set; }

        [Required]
        [Display(Name = "ImageServiceStatus")]
        public string ImageServiceStatus { get; set; }

        [Required]
        [Display(Name = "StudentsList")]
        public List<Student> StudentsList { get; set; }
        #endregion

        public ImageServiceDetails()
        {
            UpdateStudentList();

            //Create command to get echo from server.
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.EchoCommand, null, "");
            try
            {
                TCPClientChannel.GetTCPClientChannel().DisconnectClientChannel();
                //Send echo command.
                string message = TCPClientChannel.GetTCPClientChannel().SendAndReceive(command);
                //Deserialize return object.
                ConfigurationRecieveEventArgs returnParam =
                     JsonConvert.DeserializeObject<ConfigurationRecieveEventArgs>(message);
                //Check if we get ack.
                if ((ConfigurationEnum)returnParam.ConfigurationID == ConfigurationEnum.Ack)
                {
                    ImageServiceStatus = "ON";
                    command = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, null, "");
                    //Send GetConfig command.
                    message = TCPClientChannel.GetTCPClientChannel().SendAndReceive(command);
                    //Deserialize return object.
                    returnParam =
                         JsonConvert.DeserializeObject<ConfigurationRecieveEventArgs>(message);
                    OutputDirPath = returnParam.Args[0];

                    //Create list of extention
                    m_filesExtention = new List<string>
                    {
                        ".jpg", ".png", ".gif", ".bmp"
                    };

                    UpdateNumberOfPictures();
                }
            }
            catch (Exception e)
            {
                //If there was exception - it means that there is no connection.
                ImageServiceStatus = "OFF";
            }
        }


        private void UpdateNumberOfPictures()
        {
            int counter = 0;
            //Get files in directory.
            string[] files = Directory.GetFiles(OutputDirPath, "*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                try
                {
                    //Extract file extension.
                    string fileExtension = Path.GetExtension(file);
                    //Check if file has valid extension, case insesetive.
                    if (m_filesExtention.FindIndex(x => x.Equals(fileExtension, StringComparison.OrdinalIgnoreCase)) != -1)
                    {
                        counter++;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            counter = counter / 2;
            NumberOfPictures = counter;
        }

        private void UpdateStudentList()
        {
            const string studentInfoFile = "studentsInfo.txt";
            StudentsList = new List<Student>();

            string path = HttpContext.Current.Server.MapPath("~/App_Data/" + studentInfoFile);
            var lines = File.ReadLines(path);
            foreach (string line in lines)
            {
                string[] args = line.Split(' ');
                Student newStudent = new Student(args[0], args[1], args[2]);
                StudentsList.Add(newStudent);
            }
        }

    }
}