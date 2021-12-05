using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BiscuitQualityAssuaranceSystem.Models;
using BiscuitQualityAssuaranceSystem.Utilities;

namespace BiscuitQualityAssuaranceSystem.Controllers
{
    [Authorize(Roles = "Admin, Production Manager")]
    public class Shift_ManagerController : Controller
    {
        private ModelQualityAssuaranceDB db = new ModelQualityAssuaranceDB();

        // GET: Shift_Manager
        public ActionResult Index()
        {
            return View(db.Shift_Manager.ToList());
        }

        // GET: Shift_Manager/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift_Manager shift_Manager = db.Shift_Manager.Find(id);
            if (shift_Manager == null)
            {
                return HttpNotFound();
            }
            return View(shift_Manager);
        }

        // GET: Shift_Manager/Create
        public ActionResult Create()
        {
            var ShiftManagerID = AutoIDs.autoId("Shift_Manager", db);

            ViewBag.SM_id = ShiftManagerID;

            return View();
        }

        // POST: Shift_Manager/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SM_id,SM_F_name,SM_L_name,Tel_no,SM_User_Name")] Shift_Manager shift_Manager)
        {
            shift_Manager.SM_id = AutoIDs.autoId("Shift_Manager", db);

            if (ModelState.IsValid)
            {
                db.Shift_Manager.Add(shift_Manager);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shift_Manager);
        }

        // GET: Shift_Manager/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift_Manager shift_Manager = db.Shift_Manager.Find(id);
            if (shift_Manager == null)
            {
                return HttpNotFound();
            }
            return View(shift_Manager);
        }

        // POST: Shift_Manager/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SM_id,SM_F_name,SM_L_name,Tel_no,SM_User_Name")] Shift_Manager shift_Manager)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shift_Manager).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shift_Manager);
        }

        // GET: Shift_Manager/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift_Manager shift_Manager = db.Shift_Manager.Find(id);
            if (shift_Manager == null)
            {
                return HttpNotFound();
            }
            return View(shift_Manager);
        }

        // POST: Shift_Manager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Shift_Manager shift_Manager = db.Shift_Manager.Find(id);
            db.Shift_Manager.Remove(shift_Manager);
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
