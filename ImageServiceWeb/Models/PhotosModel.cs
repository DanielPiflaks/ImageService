using Communication;
using Infrastructure.Enums;
using Infrastructure.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class PhotosModel
    {
        #region Properties
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "PhotosList")]
        public List<Photo> PhotosList { get; set; }

        private string m_outputDirPath;
        public string OutputDirPath
        {
            get { return m_outputDirPath; }
            set { m_outputDirPath = value; }
        }
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public PhotosModel()
        {
            string message;
            ConfigurationRecieveEventArgs returnParam;
            PhotosList = new List<Photo>();

            try
            {
                //Initialize command.
                CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, null, "");
                //Send GetConfig command.
                message = TCPClientChannel.GetTCPClientChannel().SendAndReceive(command);
                //Deserialize return object.
                returnParam = JsonConvert.DeserializeObject<ConfigurationRecieveEventArgs>(message);
                //Get outputdir.
                OutputDirPath = returnParam.Args[0];
                //Update photos list.
                UpdatePhotosList();
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Update photos list.
        /// </summary>
        private void UpdatePhotosList()
        {
            //Save path to remove.
            string pathToRemove = Path.GetDirectoryName(OutputDirPath);

            //Get files in directory.
            string[] thumbnailPhotos = Directory.GetFiles(OutputDirPath + "\\Thumbnails", "*", SearchOption.AllDirectories);
            string[] normalSizePhotos = Directory.GetFiles(OutputDirPath, "*", SearchOption.AllDirectories);

            //Remove thumbnail photos from normal size photos.
            foreach (string file in thumbnailPhotos)
            {
                normalSizePhotos = normalSizePhotos.Where(val => val != file).ToArray();
            }

            foreach (string fileNormal in normalSizePhotos)
            {
                try
                {
                    //Get file name.
                    string fileNameNormal = Path.GetFileNameWithoutExtension(fileNormal);
                    string fileThumbnail = null;
                    //Find thumbnail file.
                    foreach (var file in thumbnailPhotos)
                    {
                        if (file.Contains(fileNameNormal))
                        {
                            fileThumbnail = file;
                            break;
                        }
                    }
                    //Get directory name.
                    string parentDirectory = Path.GetDirectoryName(fileNormal);
                    //Get month and year.
                    string month = Path.GetFileName(parentDirectory);
                    string year = Path.GetFileName(Path.GetDirectoryName(parentDirectory));
                    //Create file path to handle from web.
                    string fileThumbnailPath = @"~\" + fileThumbnail.Replace(pathToRemove, "");
                    string fileNormalPath = @"~\" + fileNormal.Replace(pathToRemove, "");
                    //Create new photo.
                    Photo newPhoto = new Photo(fileNameNormal, year, month, fileThumbnailPath,
                        fileNormalPath, fileThumbnail, fileNormal);
                    //Add new photo to list.
                    PhotosList.Add(newPhoto);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Return photo.
        /// </summary>
        /// <param name="photoName">Wanted photo name.</param>
        /// <returns>Wanted photo</returns>
        public Photo GetPhotoByName(string photoName)
        {
            foreach (Photo photo in PhotosList)
            {
                if (photo.Name == photoName)
                {
                    return photo;
                }
            }
            return null;
        }

        /// <summary>
        /// Deletes given photo.
        /// </summary>
        /// <param name="photoToDelete"></param>
        public void DeletePhoto(Photo photoToDelete)
        {
            //Delete files.
            File.Delete(photoToDelete.OriginPathThumbnail);
            File.Delete(photoToDelete.OriginPathFullSize);

            PhotosList.Remove(photoToDelete);
        }
    }
}