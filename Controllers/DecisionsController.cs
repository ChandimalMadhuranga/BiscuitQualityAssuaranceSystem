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
using BiscuitQualityAssuaranceSystem.Reports;
using BiscuitQualityAssuaranceSystem.Utilities;
using Microsoft.Reporting.WebForms;
using static BiscuitQualityAssuaranceSystem.Models.Complaint;

namespace BiscuitQualityAssuaranceSystem.Controllers
{
    [Authorize(Roles = "Admin, Production Manager, Shift Manager, Quality Checker")]
    public class DecisionsController : Controller
    {
        private ModelQualityAssuaranceDB db = new ModelQualityAssuaranceDB();

        // GET: Decisions
        public ActionResult Index()
        {
            var decisions = db.Decisions.Include(d => d.Complaint).Include(d => d.Shift_Manager);
            return View(db.Decisions.ToList());
        }

        // GET: Decisions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Decision decision = db.Decisions.Find(id);
            if (decision == null)
            {
                return HttpNotFound();
            }
            return View(decision);
        }

        // GET: Decisions/Create
        public ActionResult Create()
        {
            var DecisionID = AutoIDs.autoId("Decision", db);

            ViewBag.Des_ID = DecisionID;
            ViewBag.Com_ID = new SelectList(db.Complaints, "Com_ID", "Description");
            ViewBag.SM_id = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name");
            return View();
        }

        public ActionResult UpdateManagerDecision()
        {
            ViewBag.Com_ID = new SelectList(db.Complaints, "Com_ID", "Description");
            ViewBag.SM_id = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name");
            return View();
        }

        // POST: Decisions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Des_ID,Des_Description,Dec_date,SM_id,Com_ID,Dec_Status")] Decision decision)
        {
            decision.Des_ID = AutoIDs.autoId("Decision", db);

            if (ModelState.IsValid)
            {
                db.Decisions.Add(decision);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Com_ID = new SelectList(db.Complaints, "Com_ID", "Description", decision.Com_ID);
            ViewBag.SM_id = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name", decision.SM_id);
            return View(decision);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateManagerDecision(Decision decision, string id)
        {
            var username = User.Identity.Name;
            Shift_Manager shift_Manager = db.Shift_Manager.Where(a => a.SM_User_Name == username).FirstOrDefault();

            //var decMaxCount = db.Decisions.Count() + 1;
            //var decisionId = $"DES{decMaxCount}";           

            decision.Com_ID = id;
            decision.Dec_date = DateTime.Now;
            decision.SM_id = shift_Manager.SM_id;
            decision.Des_ID = AutoIDs.autoId("Decision", db);

            db.Decisions.Add(decision);
            Complaint complaint = db.Complaints.Where(a => a.Com_ID == id).FirstOrDefault();
            complaint.Com_Status = Complaint.ComplaintStatus.Completed;                
            db.SaveChanges();
            return RedirectToAction("ManagerTasks", "Complaints");

        }

        //public ActionResult UpdateManagerDecisionStatus(ModelQualityAssuaranceDB db )
        //{
        //    var username = User.Identity.Name;
        //    Shift_Manager shift_Manager = db.Shift_Manager.Where(a => a.SM_User_Name == username).FirstOrDefault();
        //    var complaints_Opens = db.Complaints.Include(c => c.Product).Include(c => c.Quality_Checker).Include(c => c.Shift_Manager).Where(a => a.SM_ID == shift_Manager.SM_id && a.Com_Status == ComplaintStatus.Open);


        //    //var decMaxCount = db.Decisions.Count() + 1;
        //    //var decisionId = $"DES{decMaxCount}";     

        //    //var status = 2;

        //    //complaint.
        //    // db.Complaints.Add();
        //   // Complaint complaint.Com_ID = id;
        //   // complaint.Dec_date = DateTime.Now;
        //    //complaint.SM_id = shift_Manager.SM_id;
        //    //complaint.Des_ID = AutoIDs.autoId("Decision", db);

        //   // db.Decisions.Add(decision);
        //   // complaint = db.Complaints.Where(a => a.Com_ID == id).FirstOrDefault();
        //    //complaint.Com_Status = Complaint.ComplaintStatus.Inprogress;
        //    //db.SaveChanges();
        //    //return RedirectToAction("ManagerTasks", "Complaints");

        //}

        // GET: Decisions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Decision decision = db.Decisions.Find(id);
            if (decision == null)
            {
                return HttpNotFound();
            }
            ViewBag.Com_ID = new SelectList(db.Complaints, "Com_ID", "Description", decision.Com_ID);
            ViewBag.SM_id = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name", decision.SM_id);
            return View(decision);
        }

        // POST: Decisions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Des_ID,Des_Description,Dec_date,SM_id,Com_ID,Dec_Status")] Decision decision)
        {
            if (ModelState.IsValid)
            {
                db.Entry(decision).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Com_ID = new SelectList(db.Complaints, "Com_ID", "Description", decision.Com_ID);
            ViewBag.SM_id = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name", decision.SM_id);
            return View(decision);
        }

        // GET: Decisions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Decision decision = db.Decisions.Find(id);
            if (decision == null)
            {
                return HttpNotFound();
            }
            return View(decision);
        }

        // POST: Decisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Decision decision = db.Decisions.Find(id);
            db.Decisions.Remove(decision);
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

        public ActionResult ReportDecisionsFilter()
        {            
            return View();
        }

        // Collect data for decision report from Product,Complaint, Decision, Quality Checker and Shift Manager tables
        public ActionResult ReportDecisions(DateTime Dec_date)
        {
            string date = Dec_date.ToString("MMMM");
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(1000);
            reportViewer.Height = Unit.Percentage(1000);
            var connectionString = ConfigurationManager.ConnectionStrings["ModelQualityAssuaranceDB1"].ConnectionString;
            SqlConnection conx = new SqlConnection(connectionString);
            conx.Open();
            string query = "SELECT d.Des_ID,p.Product_name, qc.Qc_F_name, qc.Qc_L_name, c.Description, c.Com_Date, sm.SM_F_name, sm.SM_L_name, d.Dec_date, d.Des_Description FROM Product AS p, Quality_Checker AS qc,Shift_Manager AS sm, Complaint AS c, Decision AS d WHERE p.Product_code = c.Product_code and qc.Qc_id = c.QC_ID and sm.SM_id = c.SM_ID and c.Com_ID = d.Com_ID and format(d.Dec_date,'MMMM')='"+ date + "'";
            SqlCommand cmd = new SqlCommand(query, conx);
            SqlDataReader rd = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rd);
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\DecisionReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ProductDataset", dt));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ComplaintDataSet", dt));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DecisionDataSet", dt));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("SMDataSet", dt));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("QCDataSet", dt));
            ViewBag.ReportViewer = reportViewer;
            conx.Close();
            return View();
        }
    }
}
