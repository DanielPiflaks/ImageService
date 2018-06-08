using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class Photo
    {
        #region Properties
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Year")]
        public string Year { get; set; }

        [Required]
        [Display(Name = "Month")]
        public string Month { get; set; }

        [Required]
        [Display(Name = "ThumbnailPath")]
        public string ThumbnailPath { get; set; }

        [Required]
        [Display(Name = "OriginSizePath")]
        public string OriginSizePath { get; set; }

        private string m_originPathFullSize;
        public string OriginPathFullSize
        {
            get { return m_originPathFullSize; }
            set { m_originPathFullSize = value; }
        }

        private string m_originPathThumbnail;
        public string OriginPathThumbnail
        {
            get { return m_originPathThumbnail;}
            set { m_originPathThumbnail = value; }
        }


        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of photo</param>
        /// <param name="year">Year</param>
        /// <param name="month">Month</param>
        /// <param name="thumbnailPath">Path of web</param>
        /// <param name="originalSizePath">Path of web</param>
        /// <param name="originPathFullSize">Origin path</param>
        /// <param name="originPathThumbnail">Origin path</param>
        public Photo(string name, string year, string month, string thumbnailPath, string originalSizePath,
            string originPathFullSize, string originPathThumbnail)
        {
            Name = name;
            Year = year;
            Month = month;
            ThumbnailPath = thumbnailPath;
            OriginSizePath = originalSizePath;
            OriginPathFullSize = originPathFullSize;
            OriginPathThumbnail = originPathThumbnail;
        }
    }
}