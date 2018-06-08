using ImageServiceWeb.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class DetailsController : Controller
    {
        #region Properties
        public ImageServiceDetails serviceDetails;

        #endregion

        /// <summary>
        /// GET student list details.
        /// </summary>
        /// <returns></returns>
        public ActionResult ServiceDetails()
        {
            return View(serviceDetails.StudentsList);
        }

        /// <summary>
        /// constructor.
        /// </summary>
        public DetailsController()
        {
            serviceDetails = new ImageServiceDetails();
            ViewBag.ImageServiceStatus = serviceDetails.ImageServiceStatus;
            ViewBag.NumberOfPictures = serviceDetails.NumberOfPictures;
        }
    }
}