using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageServiceWeb.Models;

namespace ImageServiceWeb.Controllers
{
    public class LogInfoController : Controller
    {
        LogInfoModel logInfoModel;

        // GET: Log
        public ActionResult LogInfo()
        {
            return View(logInfoModel.LogMessages);
        }

        public LogInfoController()
        {
            logInfoModel = new LogInfoModel();
        }

        [HttpPost]
        public ActionResult LogInfo(FormCollection form)
        {
            string type = form["filterType"];
            if (type == "")
            {
                return View(logInfoModel.LogMessages);
            }
            else
            {
                List<Log> filteredLogs = new List<Log>();
                foreach (Log current in logInfoModel.LogMessages)
                {
                    int comp = (int) current.Status;
                    if (comp.ToString() == type)
                    {
                        filteredLogs.Add(current);
                    }
                }
                return View(filteredLogs);
            }
        }
    }
}