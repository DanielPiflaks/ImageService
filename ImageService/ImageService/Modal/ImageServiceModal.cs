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
        //we init this once so that if the function is repeatedly called
        //it isn't stressing the garbage man
        private static Regex r = new Regex(":");
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size
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

        public ImageServiceModal(string outputFolder, int thumbnailSize)
        {
            OutputFolder = outputFolder;
            ThumbnailSize = thumbnailSize;
        }

        public string AddFile(string path, out bool result)
        {
            try
            {
                if (File.Exists(path))
                {
                    if (!Directory.Exists(m_OutputFolder))
                    {
                        DirectoryInfo newOutputFolder = Directory.CreateDirectory(m_OutputFolder);
                        newOutputFolder.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    }
                    else
                    {
                        DirectoryInfo existingOutDir = new DirectoryInfo(m_OutputFolder);
                        if (!existingOutDir.Attributes.HasFlag(FileAttributes.Hidden))
                        {
                            existingOutDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                        }
                    }

                    DateTime ct = GetFileCreationTime(path);
                    int year = ct.Year;
                    int month = ct.Month;

                    string thumbnailPath = m_OutputFolder + @"\Thumbnails";
                    if (!Directory.Exists(thumbnailPath))
                    {
                        Directory.CreateDirectory(thumbnailPath);
                    }

                    CreateThumbnailFile(path, thumbnailPath, year, month);

                    string newPath = MoveFileToOutputDir(path, m_OutputFolder, year, month);

                    result = true;
                    string returnMessage = "File " + Path.GetFileName(path) + " moved to " + newPath + " and thumnail size created";
                    return returnMessage;
                }
                else
                {
                    throw new Exception("File does not exist");
                }
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }

        private string MoveFileToOutputDir(string filePath, string outputDir, int year, int month)
        {
            string newPath = CreateFileCorrectDir(outputDir, year, month);
            try
            {
                string fileName = Path.GetFileName(filePath);
                newPath = newPath + "\\" + fileName;
                if (File.Exists(newPath))
                {
                    File.Delete(filePath);
                }
                else
                {
                    File.Move(filePath, newPath);
                }
                return newPath;
            }
            catch
            {
                throw new Exception("Failed moving file: " + filePath + "to output directory");
            }
        }

        private void CreateThumbnailFile(string filePath, string outputDir, int year, int month)
        {
            string newPath = CreateFileCorrectDir(outputDir, year, month);
            try
            {
                newPath = newPath + "\\" + Path.GetFileName(filePath);

                if (!File.Exists(newPath))
                {
                    Image originImage = Image.FromFile(filePath);
                    Image thumbnailSize = (Image)(new Bitmap(originImage, new Size(this.m_thumbnailSize, this.m_thumbnailSize)));

                    thumbnailSize.Save(newPath);
                    thumbnailSize.Dispose();
                    originImage.Dispose();
                }
            }
            catch
            {
                throw new Exception("Failed creating thumbnail file for "+ filePath);
            }
        }

        private string CreateFileCorrectDir(string outputDir, int year, int month)
        {
            string dirByYear = outputDir + @"\" + year;
            string dirByYearMonth = dirByYear + @"\" + month;
            CreateFolder(dirByYear);
            CreateFolder(dirByYearMonth);
            return dirByYearMonth;
        }

        private DateTime GetFileCreationTime(string filename)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan offset = currentTime - currentTime.ToUniversalTime();
            try
            {
                DateTime creationTime = File.GetLastWriteTimeUtc(filename) + offset;
                return creationTime;
            }
            catch
            {
                throw new Exception("Failed gatting creation time from " + filename);
            }
        }

        public void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception e)
                {
                    throw new Exception("Unabled to create folder " + path);
                }
            }

        }

    }

}
