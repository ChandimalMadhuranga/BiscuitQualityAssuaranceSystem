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
    [Authorize(Roles = "Admin, Production Manager,Shift Manager")]
    public class ProductsController : Controller
    {
        private ModelQualityAssuaranceDB db = new ModelQualityAssuaranceDB();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.Shift_Manager);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            var ProductID = AutoIDs.autoId("Product", db);

            ViewBag.Product_code = ProductID;
            ViewBag.ProductCat_name = new SelectList(db.Categories, "cat_name", "cat_name");
            ViewBag.SM_ID = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Product_code,Product_name,ProductCat_name,SM_ID")] Product product)
        {
            product.Product_code = AutoIDs.autoId("Product", db);

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductCat_name = new SelectList(db.Categories, "cat_name", "cat_desc", product.ProductCat_name);
            ViewBag.SM_ID = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name", product.SM_ID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductCat_name = new SelectList(db.Categories, "cat_name", "cat_desc", product.ProductCat_name);
            ViewBag.SM_ID = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name", product.SM_ID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Product_code,Product_name,ProductCat_name,SM_ID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCat_name = new SelectList(db.Categories, "cat_name", "cat_desc", product.ProductCat_name);
            ViewBag.SM_ID = new SelectList(db.Shift_Manager, "SM_id", "SM_F_name", product.SM_ID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
