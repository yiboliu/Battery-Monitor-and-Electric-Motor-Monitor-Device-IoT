using BatteryMotorMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BatteryMotorMonitor.ViewModels;


namespace BatteryMotorMonitor.Controllers
{
    public class HomeController : Controller
    {

        private IoTBatteryDataEntities db = new IoTBatteryDataEntities();

        public async Task<ActionResult> Index()
        {
            string result = await HelperClasses.BatteryMLService.GetMLResponse();
            HomeViewModel hvm = new HomeViewModel
            {
                response = result
            };
            return View(hvm);
        }
    }
}