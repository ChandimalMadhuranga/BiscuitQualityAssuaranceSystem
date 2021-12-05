using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BiscuitQualityAssuaranceSystem.Models;
using BiscuitQualityAssuaranceSystem.Utilities;

namespace BiscuitQualityAssuaranceSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly ModelQualityAssuaranceDB db = new ModelQualityAssuaranceDB();

        
        public ActionResult Welcome()
        {
            return View();
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        } 

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult UserRegistration()
        {
            return RedirectToAction("Index", "UserRegistration");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {              

                var e_password = Encription.GetMD5(user.User_Password);
                bool IsValidUser = db.Users.Any(usr => usr.User_Name.ToLower() ==
                user.User_Name.ToLower() && usr.User_Password == e_password);

                if (IsValidUser)
                {
                    FormsAuthentication.SetAuthCookie(user.User_Name, false);
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Invalid Username or Password");
            return View();

        }

        public ActionResult Register()
        {
            var EmployeeID = AutoIDs.autoId("User", db);

            ViewBag.User_Employee_ID = EmployeeID;
            ViewBag.User_Role_ID = new SelectList(db.User_Roles, "User_Role_ID", "User_Role_Type");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "User_Employee_ID,User_Name,User_F_Name,User_L_Name,User_Email,User_Mob_Number,User_Role_ID,User_Password,User_Registration_Date")] User user)
        {
            user.User_Employee_ID = AutoIDs.autoId("User", db);

            if (ModelState.IsValid)
            {
                user.User_Password = Encription.GetMD5(user.User_Password);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");

            }
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


    }
}