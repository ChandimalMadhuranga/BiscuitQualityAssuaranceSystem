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
    public class UserRegistrationController : Controller
    {
        private ModelQualityAssuaranceDB db = new ModelQualityAssuaranceDB();

        // GET: UserRegistration
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.User_Roles);
            return View(users.ToList());
        }

        // GET: UserRegistration/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: UserRegistration/Create
        public ActionResult Create()
        {
            var EmployeeID = AutoIDs.autoId("User", db);

            ViewBag.User_Employee_ID = EmployeeID;
            ViewBag.User_Role_ID = new SelectList(db.User_Roles, "User_Role_ID", "User_Role_Type");
            return View();
        }

        // POST: UserRegistration/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "User_Employee_ID,User_Name,User_F_Name,User_L_Name,User_Email,User_Mob_Number,User_Role_ID,User_Password,User_Registration_Date")] User user)
        {
            user.User_Employee_ID = AutoIDs.autoId("User", db);

            if (ModelState.IsValid)
            {            

                user.User_Password = Encription.GetMD5(user.User_Password);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.User_Role_ID = new SelectList(db.User_Roles, "User_Role_ID", "User_Role_Type", user.User_Role_ID);
            return View(user);
        }

        // GET: UserRegistration/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_Role_ID = new SelectList(db.User_Roles, "User_Role_ID", "User_Role_Type", user.User_Role_ID);
            return View(user);
        }

        // POST: UserRegistration/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User_Employee_ID,User_Name,User_F_Name,User_L_Name,User_Email,User_Mob_Number,User_Role_ID,User_Password,User_Registration_Date")] User user)
        {
            if (ModelState.IsValid)
            {
                user.User_Password = Encription.GetMD5(user.User_Password);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User_Role_ID = new SelectList(db.User_Roles, "User_Role_ID", "User_Role_Type", user.User_Role_ID);
            return View(user);
        }

        // GET: UserRegistration/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: UserRegistration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
