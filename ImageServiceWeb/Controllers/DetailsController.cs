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

        public ActionResult ServiceDetails()
        {
            return View(serviceDetails.StudentsList);
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public DetailsController()
        {
            serviceDetails = new ImageServiceDetails();
            ViewBag.ImageServiceStatus = serviceDetails.ImageServiceStatus;
            ViewBag.NumberOfPictures = serviceDetails.NumberOfPictures;
        }
    }
}