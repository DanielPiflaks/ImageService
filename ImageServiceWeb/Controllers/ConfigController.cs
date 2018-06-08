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
        private static bool firstBuildFlag = true;

        public ActionResult Config()
        {
            return View(configModel);
        }

        public ActionResult AskToRemoveHandler(string handler)
        {
            m_handlerToRemove = handler;
            return RedirectToAction("RemovePage");
        }

        public ActionResult RemovePage()
        {
            return View(configModel);
        }

        [HttpPost]
        public ActionResult OKClick()
        {
            configModel.RemoveHandler(m_handlerToRemove);
            return RedirectToAction("Config");
        }

        public ActionResult CancelClick()
        {
            return RedirectToAction("Config");
        }

        public ConfigController()
        {
            //if (firstBuildFlag)
            //{
                configModel = new ConfigModel();

                ViewBag.OutputDir = configModel.OutputDir;
                ViewBag.ThumbnailSize = configModel.ThumbnailSize;
                ViewBag.LogName = configModel.LogName;
                ViewBag.SourceName = configModel.SourceName;
                firstBuildFlag = false;
            //}
        }
    }
}