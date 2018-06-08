using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class ConfigController : Controller
    {
        public static ConfigModel configModel;
        private static string m_handlerToRemove;

        /// <summary>
        /// GET- config.
        /// </summary>
        /// <returns></returns>
        public ActionResult Config()
        {
            return View(configModel);
        }

        /// <summary>
        /// handle requset of removing handler.
        /// </summary>
        /// <param name="handler"> wanted handler to remove.</param>
        /// <returns>redirect to remove page.</returns>
        public ActionResult AskToRemoveHandler(string handler)
        {
            m_handlerToRemove = handler;
            return RedirectToAction("RemovePage");
        }

        /// <summary>
        /// GET to removePage view.
        /// </summary>
        /// <returns></returns>
        public ActionResult RemovePage()
        {
            return View(configModel);
        }

        /// <summary>
        /// handle in case of click to remove.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OKClick()
        {
            configModel.RemoveHandler(m_handlerToRemove);
            return RedirectToAction("Config");
        }

        /// <summary>
        /// handle in case of click to cancel.
        /// </summary>
        /// <returns></returns>
        public ActionResult CancelClick()
        {
            return RedirectToAction("Config");
        }

        /// <summary>
        /// constructor.
        /// </summary>
        public ConfigController()
        {
                configModel = new ConfigModel();

                ViewBag.OutputDir = configModel.OutputDir;
                ViewBag.ThumbnailSize = configModel.ThumbnailSize;
                ViewBag.LogName = configModel.LogName;
                ViewBag.SourceName = configModel.SourceName;
        }
    }
}