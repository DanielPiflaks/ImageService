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
            if (Directory.Exists(path))
            {
                if (!Directory.Exists(m_OutputFolder))
                {
                    DirectoryInfo newOutputFolder = Directory.CreateDirectory(m_OutputFolder);
                    newOutputFolder.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }

                DateTime ct = File.GetCreationTime(path);
                int year = ct.Year;
                int month = ct.Month;

                MoveFileToOutputDir(path, m_OutputFolder, year, month);

                string thumbnailPath = m_OutputFolder + @"\Thumbnails";
                if (!Directory.Exists(thumbnailPath))
                {
                    Directory.CreateDirectory(thumbnailPath);
                }

                CreateThumbnailFile(path, thumbnailPath, year, month);
            }
            result = true;
            return "bla";
        }

        private void MoveFileToOutputDir(string filePath, string outputDir, int year, int month)
        {
            string newPath = CreateFileCorrectDir(outputDir, year, month);
            File.Move(filePath, newPath);
        }

        private void CreateThumbnailFile(string filePath, string outputDir, int year, int month)
        {
            string newPath = CreateFileCorrectDir(outputDir, year, month);
            Image originImage = Image.FromFile(filePath);
            Image thumbnailSize = (Image)(new Bitmap(originImage, new Size(this.m_thumbnailSize, this.m_thumbnailSize)));
            thumbnailSize.Save(newPath);
        }

        private string CreateFileCorrectDir(string outputDir, int year, int month)
        {
            string dirByYear = outputDir + @"\" + year;
            string dirByYearMonth = dirByYear + @"\" + month;
            if (!Directory.Exists(dirByYear))
            {
                Directory.CreateDirectory(dirByYear);
                Directory.CreateDirectory(dirByYearMonth);
            }
            else if (!Directory.Exists(dirByYearMonth))
            {
                Directory.CreateDirectory(dirByYearMonth);
            }
            return dirByYearMonth;
        }
    }
}
