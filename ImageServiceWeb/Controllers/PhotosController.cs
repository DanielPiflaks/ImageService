using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class PhotosController : Controller
    {
        #region Propeties
        public PhotosModel photosModel;
        public static Photo m_selectedPhoto;
        #endregion

        /// <summary>
        /// Shows selected photo.
        /// </summary>
        /// <param name="selectedPhotoName">Selected photo name.</param>
        /// <returns>Views selected photo.</returns>
        public ActionResult ViewSelectedPhoto(string selectedPhotoName)
        {  
            //Get selected photo and save it.
            m_selectedPhoto = photosModel.GetPhotoByName(selectedPhotoName);
            //View.
            return View(m_selectedPhoto);
        }

        /// <summary>
        /// Delte selected photo.
        /// </summary>
        /// <param name="selectedPhotoName">Selected photo to view.</param>
        /// <returns>View it.</returns>
        public ActionResult DeleteSelectedPhoto(string selectedPhotoName)
        {
            //Get selected photo and save it.
            m_selectedPhoto = photosModel.GetPhotoByName(selectedPhotoName);
            //View.
            return View(m_selectedPhoto);
        }

        /// <summary>
        /// Uppon OKClick button.
        /// </summary>
        /// <returns>Redirect to photos.</returns>
        [HttpPost]
        public ActionResult OKClick()
        {
            photosModel.DeletePhoto(m_selectedPhoto);
            return RedirectToAction("Photos");
        }

        /// <summary>
        /// Uppon CancelClick button.
        /// </summary>
        /// <returns>Redirect to photos.</returns>
        [HttpPost]
        public ActionResult CancelClick()
        {
            return RedirectToAction("Photos");
        }

        /// <summary>
        /// Shows photos view page.
        /// </summary>
        /// <returns>View of photos.</returns>
        public ActionResult Photos()
        {
            return View(photosModel.PhotosList);
        }
        /// <summary>
        /// Consturctor.
        /// </summary>
        public PhotosController()
        {
            photosModel = new PhotosModel();
        }
    }
}