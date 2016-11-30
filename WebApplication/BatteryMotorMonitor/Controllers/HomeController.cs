using BatteryMotorMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BatteryMotorMonitor.Controllers
{
    public class HomeController : Controller
    {

        private IoTBatteryDataEntities db = new IoTBatteryDataEntities();

        public ActionResult Index()
        {
            return View();
        }
    }
}