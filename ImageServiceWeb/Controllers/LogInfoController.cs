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
        public ActionResult Logs(FormCollection form)
        {
            string type = form["type"].ToString();
            if (type == "Type")
            {
                return View(logInfoModel.LogMessages);
            }
            else
            {
                int i = 0;
                List<Log> filteredLogs = new List<Log>();
                while (logInfoModel.LogMessages != null)
                {
                    Log current = logInfoModel.LogMessages[i];
                    if (current.Status.ToString() == type)
                    {
                        filteredLogs.Add(current);
                    }
                    i++;
                }
                return View(filteredLogs);
            }
        }
    }
}