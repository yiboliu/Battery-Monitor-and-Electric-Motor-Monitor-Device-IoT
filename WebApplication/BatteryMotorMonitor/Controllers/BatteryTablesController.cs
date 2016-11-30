using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BatteryMotorMonitor.Models;
using LINQtoCSV;
using RDotNet;

namespace BatteryMotorMonitor.Controllers
{
    public class BatteryTablesController : Controller
    {
        private IoTBatteryDataEntities db = new IoTBatteryDataEntities();

        // GET: BatteryTables
        public ActionResult Index()
        {
            return View(db.BatteryTables.ToList());
        }

        public ActionResult ViewDataReporting()
        {
            return View();
        }

        public void refreshDataAnalytics()
        {
            List<BatteryTable> batteryDataPoints = db.BatteryTables.ToList();
            CsvFileDescription outputFileDescription = new CsvFileDescription
            {
                SeparatorChar = '\t'
            };
            CsvContext cc = new CsvContext();
            cc.Write(
                batteryDataPoints,
                "c:/Users/RunFranks525/Desktop/batteryDataPoints.csv",
                outputFileDescription);

            //Perform R Analysis
            REngine engine = REngine.GetInstance();
            engine.Initialize();


            DataFrame dataSet = engine.Evaluate("C:/Users/RunFranks525/Desktop/batteryDataPoints.csv").AsDataFrame();
            int i = 0;
        }

        // GET: BatteryTables/Details/5
        public ActionResult Details(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BatteryTable batteryTable = db.BatteryTables.Find(id);
            if (batteryTable == null)
            {
                return HttpNotFound();
            }
            return View(batteryTable);
        }

        // GET: BatteryTables/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BatteryTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DateStamp,BatteryTemperature,BatteryCurrent,BatteryVoltage,BatteryPower")] BatteryTable batteryTable)
        {
            if (ModelState.IsValid)
            {
                db.BatteryTables.Add(batteryTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(batteryTable);
        }

        // GET: BatteryTables/Edit/5
        public ActionResult Edit(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BatteryTable batteryTable = db.BatteryTables.Find(id);
            if (batteryTable == null)
            {
                return HttpNotFound();
            }
            return View(batteryTable);
        }

        // POST: BatteryTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DateStamp,BatteryTemperature,BatteryCurrent,BatteryVoltage,BatteryPower")] BatteryTable batteryTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(batteryTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(batteryTable);
        }

        // GET: BatteryTables/Delete/5
        public ActionResult Delete(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BatteryTable batteryTable = db.BatteryTables.Find(id);
            if (batteryTable == null)
            {
                return HttpNotFound();
            }
            return View(batteryTable);
        }

        // POST: BatteryTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DateTime id)
        {
            BatteryTable batteryTable = db.BatteryTables.Find(id);
            db.BatteryTables.Remove(batteryTable);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
