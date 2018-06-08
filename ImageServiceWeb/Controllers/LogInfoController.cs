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

        /// <summary>
        /// GET: Log- case of no params
        /// </summary>
        public ActionResult LogInfo()
        {
            // return log messages.
            return View(logInfoModel.LogMessages);
        }

        /// <summary>
        /// constructor.
        /// </summary>
        public LogInfoController()
        {
            logInfoModel = new LogInfoModel();
        }

        /// <summary>
        /// in case of user ask to filter log messages by type.
        /// </summary>
        /// <param name="form"> wanted type for filtering from user.</param>
        /// <returns> filterd log messages to view.</returns>
        [HttpPost]
        // GET: Log- case of params.
        public ActionResult LogInfo(FormCollection form)
        {
            // cast input to string.
            string type = form["filterWantedType"];
            // in case of empty choice.
            if (type == "")
            {
                //return all log messages.
                return View(logInfoModel.LogMessages);
            }
            else
            {
                // set new list of logs.
                List<Log> filteredLogs = new List<Log>();
                // run over log messages.
                foreach (Log current in logInfoModel.LogMessages)
                {
                    // extract current log type.
                    string comp = current.Status.ToString();
                    // compare with user choice (ignore upper-lower letters).
                    if (comp.Equals(type, StringComparison.OrdinalIgnoreCase))
                    {
                        // if equal- add to new list of logs.
                        filteredLogs.Add(current);
                    }
                }
                //return new list.
                return View(filteredLogs);
            }
        }
    }
}