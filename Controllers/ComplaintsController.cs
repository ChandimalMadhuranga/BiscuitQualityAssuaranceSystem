using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using BiscuitQualityAssuaranceSystem.Models;
using BiscuitQualityAssuaranceSystem.Utilities;
using BiscuitQualityAssuaranceSystem.Controllers;
using static BiscuitQualityAssuaranceSystem.Models.Complaint;

namespace BiscuitQualityAssuaranceSystem.Controllers
{
    [Authorize(Roles = "Admin,Shift Manager,Quality Checker")]
    public class ComplaintsController : Controller
    {
        private ModelQualityAssuaranceDB db = new ModelQualityAssuaranceDB();

        // GET: Complaints
        public ActionResult Index()
        {
            var complaints = db.Complaints.Include(c => c.Product).Include(c => c.Quality_Checker).Include(c => c.Shift_Manager);
            return View(complaints.ToList());
            //return View(complaints.ToList().Where(a=> a.QC_ID == "Open"));
        }

        public ActionResult QualityCheckerTasks()
        {
            var username = User.Identity.Name;
            Quality_Checker quality_Checker = db.Quality_Checker.Where(a => a.QC_User_Name == username).FirstOrDefault();
            var complaints = db.Complaints.Include(c => c.Quality_Checker).Where(a => a.QC_ID == quality_Checker.Qc_id);
            return View(complaints.ToList());            
        }

        public ActionResult ManagerTasks()
        {
           // DecisionsController objDecCon = new DecisionsController();
            
            //objDecCon.UpdateManagerDecisionStatus(db, Complaint);
            var username = User.Identity.Name;
            Shift_Manager shift_Manager = db.Shift_Manager.Where(a => a.SM_User_Name == username).FirstOrDefault();
            var complaints = db.Complaints.Include(c => c.Product).Include(c => c.Quality_Checker).Include(c => c.Shift_Manager).Where(a => a.SM_ID == shift_Manager.SM_id && a.Com_Status == ComplaintStatus.Open || a.Com_Status == ComplaintStatus.Inprogress);
           // var complaints_Inprogress = db.Complaints.Include(c => c.Product).Include(c => c.Quality_Checker).Include(c => c.Shift_Manager).Where(a => a.SM_ID == shift_Manager.SM_id && a.Com_Status == ComplaintStatus.Open);
            //var Complant_Status = db.Complaints.Add();

            return View(complaints.ToList());           
        }

        // GET: Complaints/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Complaint complaint = db.Complaints.Find(id);
            if (complaint == null)
            {
                return HttpNotFound();
            }
            return View(complaint);
        }

        // GET: Complaints/Create
        public ActionResult Create()
        {            
            
            var ComplaintID = AutoIDs.autoId("Complaint", db);     

           // ViewBag.Com_ID = new SelectList(db.Complaints, "Com_ID", "Com_ID"); 
            ViewBag.Com_ID = ComplaintID;
            ViewBag.Product_code = new SelectList(db.Products, "Product_code", "Product_name");
            ViewBag.QC_ID = new SelectList(db.Quality_Checker, "Qc_id", "Qc_F_name");
            ViewBag.SM_ID = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name");
            return View();
        }

        // POST: Complaints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Com_ID,Description,Com_Date,QC_ID,SM_ID,Com_Status,Product_code")] Complaint complaint)
        {
            complaint.Com_ID = AutoIDs.autoId("Complaint", db);

            // Find shift manager email
            Shift_Manager shift_Manager = db.Shift_Manager.Where(a => a.SM_id == complaint.SM_ID).FirstOrDefault();
            User user = db.Users.Where(a => a.User_Name == shift_Manager.SM_User_Name).FirstOrDefault();

            // Find product name
            Product product = db.Products.Where(a => a.Product_code == complaint.Product_code).FirstOrDefault();

            // Find QC name
            Quality_Checker quality_Checker = db.Quality_Checker.Where(a => a.Qc_id == complaint.QC_ID).FirstOrDefault();

            complaint.Com_Status = Complaint.ComplaintStatus.Inprogress;


            if (ModelState.IsValid)
            {
                db.Complaints.Add(complaint);
                db.SaveChanges();                
                SendEmail(shift_Manager.SM_F_name,quality_Checker.Qc_F_name, product.Product_name, complaint.Description, user.User_Email);
                ViewBag.result = "Complaint saved successfully. Mail is sent";
                return RedirectToAction("Index");
            }

            ViewBag.Com_ID = new SelectList(db.Complaints, "Com_ID", "Description", complaint.Com_ID);
            ViewBag.Product_code = new SelectList(db.Products, "Product_code", "Product_name", complaint.Product_code);
            ViewBag.QC_ID = new SelectList(db.Quality_Checker, "Qc_id", "Qc_F_name", complaint.QC_ID);
            ViewBag.SM_ID = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name", complaint.SM_ID);
            return View(complaint);
        }

        // GET: Complaints/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Complaint complaint = db.Complaints.Find(id);
            if (complaint == null)
            {
                return HttpNotFound();
            }
            ViewBag.Com_ID = new SelectList(db.Complaints, "Com_ID", "Description", complaint.Com_ID);
            ViewBag.Product_code = new SelectList(db.Products, "Product_code", "Product_name", complaint.Product_code);
            ViewBag.QC_ID = new SelectList(db.Quality_Checker, "Qc_id", "Qc_F_name", complaint.QC_ID);
            ViewBag.SM_ID = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name", complaint.SM_ID);
            return View(complaint);
        }

        // POST: Complaints/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Com_ID,Description,Com_Date,QC_ID,SM_ID,Com_Status,Product_code")] Complaint complaint)
        {
            if (ModelState.IsValid)
            {
                db.Entry(complaint).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Com_ID = new SelectList(db.Complaints, "Com_ID", "Description", complaint.Com_ID);
            ViewBag.Product_code = new SelectList(db.Products, "Product_code", "Product_name", complaint.Product_code);
            ViewBag.QC_ID = new SelectList(db.Quality_Checker, "Qc_id", "Qc_F_name", complaint.QC_ID);
            ViewBag.SM_ID = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name", complaint.SM_ID);
            return View(complaint);
        }

        // GET: Complaints/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Complaint complaint = db.Complaints.Find(id);
            if (complaint == null)
            {
                return HttpNotFound();
            }
            return View(complaint);
        }

        // POST: Complaints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Complaint complaint = db.Complaints.Find(id);
            db.Complaints.Remove(complaint);
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

        public void SendEmail(string SM_F_Name, string QC_F_Name, string Com_prd_name, string Com_Descrip, string email)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("projectucscqualitybiscuit@gmail.com", "Project@UCSC123"),
                EnableSsl = true,
            };

            string subject = "New complaint added by " + QC_F_Name + " for the product " + Com_prd_name + "";
            string body = "Hi "+ SM_F_Name + ", "+ Environment.NewLine + Environment.NewLine + "\tThere is new issue found on '" + Com_prd_name + "'. Issue is describe bellow, '" + Com_Descrip + "'. Please put your attention on this ASAP." + Environment.NewLine + Environment.NewLine + "Thanks & Regards, " + Environment.NewLine + QC_F_Name + Environment.NewLine + "(Quality Checker) ";

            smtpClient.Send("projectucscqualitybiscuit@gmail.com", email, subject,body);
        }
    }
}
