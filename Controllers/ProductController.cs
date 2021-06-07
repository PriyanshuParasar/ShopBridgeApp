using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopBridgeApp.Models;

namespace ShopBridgeApp.Controllers
{
    public class ProductController : Controller
    {
        private ShopBridgeDbContext db = new ShopBridgeDbContext();

        // GET: Product
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var s_Product = db.s_Product.Include(s => s.s_Employee);
                ViewBag.IsUserAdmin = IsUserRoleAdmin();
                return View(s_Product.ToList());
            }                
        }

        //TODO
        [NonAction]
        public bool IsUserRoleAdmin()
        {
            if (Session["Role"].Equals(1))
            {
                return true;
            }
            return false;
        }


        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            s_Product s_Product = db.s_Product.Find(id);
            if (s_Product == null)
            {
                return HttpNotFound();
            }
            return View(s_Product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.CreatedByID = new SelectList(db.s_Employee, "UserID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(s_Product s_Product)
        {
            if (db.s_Product.Any(pd => pd.ProductName == s_Product.ProductName))
            {
                ModelState.AddModelError("ProductName", "Product already exists");
            }
            if (ModelState.IsValid)
            {
                int registerValue = db.sp_AddProduct(s_Product.ProductName, s_Product.Category, s_Product.Description, s_Product.Price, s_Product.CreatedByID);
                if (registerValue == 1)
                {
                    ViewBag.Message = $"The Product {s_Product.ProductName} is added successfully";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.CreatedByID = new SelectList(db.s_Employee, "UserID", "Name", s_Product.CreatedByID);
            return View(s_Product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            s_Product s_Product = db.s_Product.Find(id);
            if (s_Product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedByID = new SelectList(db.s_Employee, "UserID", "Name", s_Product.CreatedByID);

            // TODO: need to save created byID actual one.
            TempData["CreatedByID"] = s_Product.CreatedByID;
            return View(s_Product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,Category,Description,Price,CreatedByID")] s_Product s_Product)
        {
            if (TempData.ContainsKey("CreatedByID"))
                s_Product.CreatedByID = int.Parse(TempData["CreatedByID"].ToString());
            if (ModelState.IsValid)
            {
                db.Entry(s_Product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedByID = new SelectList(db.s_Employee, "UserID", "Name", s_Product.CreatedByID);
            return View(s_Product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            s_Product s_Product = db.s_Product.Find(id);
            if (s_Product == null)
            {
                return HttpNotFound();
            }
            return View(s_Product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            s_Product s_Product = db.s_Product.Find(id);
            db.s_Product.Remove(s_Product);
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
