using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BiscuitQualityAssuaranceSystem.Models;
using BiscuitQualityAssuaranceSystem.Utilities;
using BiscuitQualityAssuaranceSystem.Reports;
using Microsoft.Reporting.WebForms;

namespace BiscuitQualityAssuaranceSystem.Controllers
{
    [Authorize(Roles = "Admin,Shift Manager, Quality Checker")]
    public class Quality_CheckerController : Controller
    {
        private ModelQualityAssuaranceDB db = new ModelQualityAssuaranceDB();

        // GET: Quality_Checker
        public ActionResult Index()
        {
            var quality_Checker = db.Quality_Checker.Include(q => q.Product).Include(q => q.Shift_Manager);
            return View(quality_Checker.ToList());
        }

        // GET: Quality_Checker/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quality_Checker quality_Checker = db.Quality_Checker.Find(id);
            if (quality_Checker == null)
            {
                return HttpNotFound();
            }
            return View(quality_Checker);
        }

        // GET: Quality_Checker/Create
        public ActionResult Create()
        {
            var QualityCheckerID = AutoIDs.autoId("Quality_Checker", db);

            ViewBag.Qc_id = QualityCheckerID;
            ViewBag.Product_code = new SelectList(db.Products, "Product_code", "Product_name");
            ViewBag.SM_ID = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name");
            return View();
        }

        // POST: Quality_Checker/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Qc_id,Qc_F_name,Qc_L_name,Qc_NIC,Product_code,SM_ID,QC_User_Name")] Quality_Checker quality_Checker)
        {
            quality_Checker.Qc_id = AutoIDs.autoId("Quality_Checker", db);

            if (ModelState.IsValid)
            {
                db.Quality_Checker.Add(quality_Checker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Product_code = new SelectList(db.Products, "Product_code", "Product_name", quality_Checker.Product_code);
            ViewBag.SM_ID = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name", quality_Checker.SM_ID);
            return View(quality_Checker);
        }

        // GET: Quality_Checker/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quality_Checker quality_Checker = db.Quality_Checker.Find(id);
            if (quality_Checker == null)
            {
                return HttpNotFound();
            }
            ViewBag.Product_code = new SelectList(db.Products, "Product_code", "Product_name", quality_Checker.Product_code);
            ViewBag.SM_ID = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name", quality_Checker.SM_ID);
            return View(quality_Checker);
        }

        // POST: Quality_Checker/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Qc_id,Qc_F_name,Qc_L_name,Qc_NIC,Product_code,SM_ID,QC_User_Name")] Quality_Checker quality_Checker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quality_Checker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Product_code = new SelectList(db.Products, "Product_code", "Product_name", quality_Checker.Product_code);
            ViewBag.SM_ID = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name", quality_Checker.SM_ID);
            return View(quality_Checker);
        }

        // GET: Quality_Checker/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quality_Checker quality_Checker = db.Quality_Checker.Find(id);
            if (quality_Checker == null)
            {
                return HttpNotFound();
            }
            return View(quality_Checker);
        }

        // POST: Quality_Checker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Quality_Checker quality_Checker = db.Quality_Checker.Find(id);
            db.Quality_Checker.Remove(quality_Checker);
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

        public ActionResult ReportQualityCheckerFilter()
        {
            ViewBag.SM_ID = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name");
            return View();
        }

        // Collect data for decision report from Product, Shift Manager and Quality Checker tables
        public ActionResult ReportQualityChecker(String SM_ID)
        {           

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(1000);
            reportViewer.Height = Unit.Percentage(1000);
            var connectionString = ConfigurationManager.ConnectionStrings["ModelQualityAssuaranceDB1"].ConnectionString;
            SqlConnection conx = new SqlConnection(connectionString);
            conx.Open();
            string query = "SELECT Product.Product_code, Quality_Checker.Qc_F_name, Quality_Checker.Qc_L_name, Shift_Manager.SM_F_name, Shift_Manager.SM_L_name, Product.Product_name FROM Product INNER JOIN Quality_Checker ON Product.Product_code = Quality_Checker.Product_code INNER JOIN Shift_Manager ON Product.SM_ID = Shift_Manager.SM_id AND Quality_Checker.SM_ID = Shift_Manager.SM_id and Shift_Manager.SM_id='" + SM_ID + "'";
            SqlCommand cmd = new SqlCommand(query, conx);
            SqlDataReader rd = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rd);
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\QualityCheckerReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetProduct", dt));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetQualityChecker", dt));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetShiftManager", dt));            
            ViewBag.ReportViewer = reportViewer;
            conx.Close();
            return View();
        }
    }
}
