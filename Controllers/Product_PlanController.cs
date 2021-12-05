using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BiscuitQualityAssuaranceSystem.Models;
using BiscuitQualityAssuaranceSystem.Reports;
using BiscuitQualityAssuaranceSystem.Utilities;
using Microsoft.Reporting.WebForms;

namespace BiscuitQualityAssuaranceSystem.Controllers
{
    [Authorize(Roles = "Admin,Production Manager,Shift Manager")]
    public class Product_PlanController : Controller
    {
        private ModelQualityAssuaranceDB db = new ModelQualityAssuaranceDB();

        // GET: Product_Plan
        public ActionResult Index()
        {
            var product_Plan = db.Product_Plan.Include(p => p.Product);
            return View(product_Plan.ToList());
        }

        // GET: Product_Plan/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Plan product_Plan = db.Product_Plan.Find(id);
            if (product_Plan == null)
            {
                return HttpNotFound();
            }
            return View(product_Plan);
        }

        // GET: Product_Plan/Create
        public ActionResult Create()
        {
            var Product_PlanID = AutoIDs.autoId("Product_Plan", db);

            ViewBag.Product_Plane_Id = Product_PlanID;
            ViewBag.Product_code = new SelectList(db.Products, "Product_code", "Product_name");
            return View();
        }

        // POST: Product_Plan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Product_Plane_Id,Product_code,Product_Date,Product_Expected_qty,Product_Qty")] Product_Plan product_Plan)
        {
            product_Plan.Product_Plane_Id = AutoIDs.autoId("Product_Plan", db);

            if (ModelState.IsValid)
            {
                db.Product_Plan.Add(product_Plan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Product_code = new SelectList(db.Products, "Product_code", "Product_name", product_Plan.Product_code);
            return View(product_Plan);
        }

        // GET: Product_Plan/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Plan product_Plan = db.Product_Plan.Find(id);
            if (product_Plan == null)
            {
                return HttpNotFound();
            }
            ViewBag.Product_code = new SelectList(db.Products, "Product_code", "Product_name", product_Plan.Product_code);
            return View(product_Plan);
        }

        // POST: Product_Plan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Product_Plane_Id,Product_code,Product_Date,Product_Expected_qty,Product_Qty")] Product_Plan product_Plan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product_Plan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Product_code = new SelectList(db.Products, "Product_code", "Product_name", product_Plan.Product_code);
            return View(product_Plan);
        }

        // GET: Product_Plan/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Plan product_Plan = db.Product_Plan.Find(id);
            if (product_Plan == null)
            {
                return HttpNotFound();
            }
            return View(product_Plan);
        }

        // POST: Product_Plan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Product_Plan product_Plan = db.Product_Plan.Find(id);
            db.Product_Plan.Remove(product_Plan);
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

        public ActionResult ReportProductPlanFilter()
        {
            return View();
        }


        // Collect data for product plan report from Product_Plan and Product tables        
        public ActionResult ReportProductPlan(DateTime Product_Date)
        {
            string date = Product_Date.ToString("MMMM");
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(1000);
            reportViewer.Height = Unit.Percentage(1000);
            var connectionString = ConfigurationManager.ConnectionStrings["ModelQualityAssuaranceDB1"].ConnectionString;
            SqlConnection conx = new SqlConnection(connectionString);
            conx.Open();
            string query = "SELECT Product.Product_name, Product_Plan.Product_Qty, Product_Plan.Product_Expected_qty FROM Product INNER JOIN Product_Plan ON Product.Product_code = Product_Plan.Product_code and Format(Product_Plan.Product_Date,'MMMM')= '" + date + "'";
            
            //string query = "SELECT Product.Product_name, Product_Plan.Product_Qty, Product_Plan.Product_Expected_qty FROM Product INNER JOIN Product_Plan ON Product.Product_code = Product_Plan.Product_code"; Format(Product_plan.P_Date,'mmmm')
            SqlCommand cmd = new SqlCommand(query, conx);
            SqlDataReader rd = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rd);
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\ProductPlanReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductDataSet", dt));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductPlanDataSet", dt));            
            ViewBag.ReportViewer = reportViewer;
            conx.Close();
            return View();
        }
    }
}
