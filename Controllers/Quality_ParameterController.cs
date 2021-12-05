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
using Microsoft.Reporting.WebForms;

namespace BiscuitQualityAssuaranceSystem.Controllers
{
    [Authorize(Roles = "Admin,Production Manager,Shift Manager,Quality Checker")]
    public class Quality_ParameterController : Controller
    {
        private ModelQualityAssuaranceDB db = new ModelQualityAssuaranceDB();

        // GET: Quality_Parameter
        public ActionResult Index()
        {
            var quality_Parameter = db.Quality_Parameter.Include(q => q.Product);
            return View(quality_Parameter.ToList());
        }

        // GET: Quality_Parameter/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quality_Parameter quality_Parameter = db.Quality_Parameter.Find(id);
            if (quality_Parameter == null)
            {
                return HttpNotFound();
            }
            return View(quality_Parameter);
        }

        // GET: Quality_Parameter/Create
        public ActionResult Create()
        {
            var QualityParameterID = AutoIDs.autoId("Quality_Parameter", db);

            ViewBag.QP_code = QualityParameterID;
            ViewBag.Product_code = new SelectList(db.Products, "Product_code", "Product_name");
            return View();
        }

        // POST: Quality_Parameter/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QP_code,Weight,Waterlevel1,Waterlevel2,Taste,Moisture,Product_code")] Quality_Parameter quality_Parameter)
        {
            quality_Parameter.QP_code = AutoIDs.autoId("Quality_Parameter", db);

            if (ModelState.IsValid)
            {
                db.Quality_Parameter.Add(quality_Parameter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Product_code = new SelectList(db.Products, "Product_code", "Product_name", quality_Parameter.Product_code);
            return View(quality_Parameter);
        }

        // GET: Quality_Parameter/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quality_Parameter quality_Parameter = db.Quality_Parameter.Find(id);
            if (quality_Parameter == null)
            {
                return HttpNotFound();
            }
            ViewBag.Product_code = new SelectList(db.Products, "Product_code", "Product_name", quality_Parameter.Product_code);
            return View(quality_Parameter);
        }

        // POST: Quality_Parameter/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QP_code,Weight,Waterlevel1,Waterlevel2,Taste,Moisture,Product_code")] Quality_Parameter quality_Parameter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quality_Parameter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Product_code = new SelectList(db.Products, "Product_code", "Product_name", quality_Parameter.Product_code);
            return View(quality_Parameter);
        }

        // GET: Quality_Parameter/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quality_Parameter quality_Parameter = db.Quality_Parameter.Find(id);
            if (quality_Parameter == null)
            {
                return HttpNotFound();
            }
            return View(quality_Parameter);
        }

        // POST: Quality_Parameter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Quality_Parameter quality_Parameter = db.Quality_Parameter.Find(id);
            db.Quality_Parameter.Remove(quality_Parameter);
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

        public ActionResult ReportQualityParameterFilter()
        {
            return View();
        }

        // Collect data for product plan report from Product_Plan and Product tables        
        public ActionResult ReportQualityParameter(String cat_name)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(1000);
            reportViewer.Height = Unit.Percentage(1000);
            var connectionString = ConfigurationManager.ConnectionStrings["ModelQualityAssuaranceDB1"].ConnectionString;
            SqlConnection conx = new SqlConnection(connectionString);
            conx.Open();
            string query = "SELECT Product.Product_name, Quality_Parameter.Weight, Quality_Parameter.Waterlevel1, Quality_Parameter.Waterlevel2, Quality_Parameter.Taste, Quality_Parameter.Moisture FROM Product INNER JOIN Quality_Parameter ON Product.Product_code = Quality_Parameter.Product_code and Product.ProductCat_name = '"+ cat_name + "'";
            SqlCommand cmd = new SqlCommand(query, conx);
            SqlDataReader rd = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rd);
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\QualityParameterReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetProduct", dt));            
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetQualityParameter", dt));
            ViewBag.ReportViewer = reportViewer;
            conx.Close();
            return View();
        }
    }
}
