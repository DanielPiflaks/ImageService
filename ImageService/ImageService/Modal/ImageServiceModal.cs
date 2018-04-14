using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private static Regex r = new Regex(":");
        // The Output Folder
        private string m_OutputFolder;
        // The Size Of The Thumbnail Size  
        private int m_thumbnailSize;             
        #endregion
        #region properties
        public int ThumbnailSize
        {
            get
            {
                return m_thumbnailSize;
            }
            set
            {
                m_thumbnailSize = value;
            }
        }
        public string OutputFolder
        {
            get
            {
                return m_OutputFolder;
            }
            set
            {
                m_OutputFolder = value;
            }
        }
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="outputFolder">Output directory.</param>
        /// <param name="thumbnailSize">Wanted thumbnail size.</param>
        public ImageServiceModal(string outputFolder, int thumbnailSize)
        {
            OutputFolder = outputFolder;
            ThumbnailSize = thumbnailSize;
        }

        /// <summary>
        /// Adds file.
        /// </summary>
        /// <param name="path">Path of file.</param>
        /// <param name="result">Result of function</param>
        /// <returns></returns>
        public string AddFile(string path, out bool result)
        {  
            try
            {   
                //Check if file exist.
                if (File.Exists(path))
                {
                    //Check if output directory does not exists.
                    if (!Directory.Exists(m_OutputFolder))
                    {
                        //Create output folder.
                        DirectoryInfo newOutputFolder = Directory.CreateDirectory(m_OutputFolder);
                        //Make it hidden.
                        newOutputFolder.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    }
                    else
                    {
                        //Get information of existing directory.
                        DirectoryInfo existingOutDir = new DirectoryInfo(m_OutputFolder);
                        //Check if directory is not hidden.
                        if (!existingOutDir.Attributes.HasFlag(FileAttributes.Hidden))
                        {
                            //Make directory hidden.
                            existingOutDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                        }
                    }
                    //Get file creation time.
                    DateTime ct = GetFileCreationTime(path);
                    //Get year of creation.
                    int year = ct.Year;
                    //Get month of creation.
                    int month = ct.Month;

                    //Create path for thumbnail size files.
                    string thumbnailPath = m_OutputFolder + @"\Thumbnails";
                    //Check if directory does not exists.
                    if (!Directory.Exists(thumbnailPath))
                    {
                        //Create directory.
                        Directory.CreateDirectory(thumbnailPath);
                    }
                    //Create thumbnail file size.
                    CreateThumbnailFile(path, thumbnailPath, year, month);
                    //Move file to new directory.
                    string newPath = MoveFileToOutputDir(path, m_OutputFolder, year, month);

                    result = true;
                    string returnMessage = "File " + Path.GetFileName(path) + " moved to " + newPath + " and thumnail size created";
                    return returnMessage;
                }
                else
                {
                    //Throw exeption if file does not exist.
                    throw new Exception("File does not exist");
                }
            }
            //Catch exception if something went wrong in the process of adding file.
            catch (Exception e)
            {
                //Update result to be false.
                result = false;
                //Return message of exception.
                return e.Message;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="outputDir"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private string MoveFileToOutputDir(string filePath, string outputDir, int year, int month)
        {
            //Create correct directory for file.
            string newPath = CreateFileCorrectDir(outputDir, year, month);
            try
            {
                //Get file name.
                string fileName = Path.GetFileName(filePath);
                //Add file name to path.
                newPath = newPath + "\\" + fileName;
                //Check if file already exists.
                if (File.Exists(newPath))
                {
                    //Delete origin file.
                    File.Delete(filePath);
                }
                else
                {
                    //Move origin file to new place.
                    File.Move(filePath, newPath);
                }
                //Return new path.
                return newPath;
            }
            catch
            {
                //Throw exception if failed to move file.
                throw new Exception("Failed moving file: " + filePath + " to output directory");
            }
        }

        /// <summary>
        /// Creates thumbnail file.
        /// </summary>
        /// <param name="filePath">Origin image to create for it thumbnail file.</param>
        /// <param name="outputDir">Where to create thumbnail file.</param>
        /// <param name="year">Year that image was created.</param>
        /// <param name="month">Month that image was created.</param>
        private void CreateThumbnailFile(string filePath, string outputDir, int year, int month)
        {
            //Create folder for thumbnail size image.
            string newPath = CreateFileCorrectDir(outputDir, year, month);
            try
            {
                //Get file name and add it to path.
                newPath = newPath + "\\" + Path.GetFileName(filePath);
                //Check if thumbnail file already exists.
                if (!File.Exists(newPath))
                {
                    //Get origin image.
                    Image originImage = Image.FromFile(filePath);
                    //Create thumbnail file.
                    Image thumbnailSize = (Image)(new Bitmap(originImage, new Size(m_thumbnailSize, m_thumbnailSize)));
                    //Save thumbnail file.
                    thumbnailSize.Save(newPath);
                    //Dispose process of thumbnailSize image.
                    thumbnailSize.Dispose();
                    //Dispode process of origin image.
                    originImage.Dispose();
                }
            }
            catch
            {
                //Throw exception if failed to create thumbnail size image.
                throw new Exception("Failed creating thumbnail file for "+ filePath);
            }
        }

        /// <summary>
        /// Creates correct directory for file.
        /// </summary>
        /// <param name="outputDir">Where to create output directory.</param>
        /// <param name="year">Which year file was created.</param>
        /// <param name="month">Which month file was created.</param>
        /// <returns>Path of new directory.</returns>
        private string CreateFileCorrectDir(string outputDir, int year, int month)
        {
            //Path of output directory by year.
            string dirByYear = outputDir + @"\" + year;
            //Path of output directory by year and month.
            string dirByYearMonth = dirByYear + @"\" + month;
            //Create folder by year.
            CreateFolder(dirByYear);
            //Create folder by year and month.
            CreateFolder(dirByYearMonth);
            //Return path.
            return dirByYearMonth;
        }

        /// <summary>
        /// Gets file creation time.
        /// </summary>
        /// <param name="filename">File name.</param>
        /// <returns>DateTime of creation time</returns>
        private DateTime GetFileCreationTime(string filename)
        {
            //Get current time.
            DateTime currentTime = DateTime.Now;
            //Calc offset.
            TimeSpan offset = currentTime - currentTime.ToUniversalTime();
            try
            {
                //Get creation time
                DateTime creationTime = File.GetLastWriteTimeUtc(filename) + offset;
                return creationTime;
            }
            catch
            {
                //Throw exception if faliled getting creation time.
                throw new Exception("Failed getting creation time from " + filename);
            }
        }

        /// <summary>
        /// Creates folder in wanted path.
        /// </summary>
        /// <param name="path">Path to create folder in.</param>
        public void CreateFolder(string path)
        {
            //Check if directory already exist.
            if (!Directory.Exists(path))
            {
                try
                {   
                    //Create directory.
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    //Throw exception if failed to create folder.
                    throw new Exception("Unabled to create folder " + path);
                }
            }

        }

    }

}
