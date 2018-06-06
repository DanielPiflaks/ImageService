using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class ConfigController : Controller
    {
        // GET: Config
        public ActionResult Config()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}