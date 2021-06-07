using ShopBridgeApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ShopBridgeApp.Controllers
{
    public class AccountController : Controller
    {
        private ShopBridgeDbContext db = new ShopBridgeDbContext();

        // GET: Account/Signup
        public ActionResult Signup()
        {
            ViewBag.Role = new SelectList(db.s_Role, "RoleId", "RoleName");

            return View();
        }

        // POST: Account/Signup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(s_Employee employee)
        {
            if (db.s_Employee.Any(emp => emp.UserName == employee.UserName))
            {
                ModelState.AddModelError("UserName", "UserName already in use. Please choose anothe UserName");
            }
            if (ModelState.IsValid)
            {
                int registerValue = db.sp_RegisterEmployee(employee.Name, employee.Role, employee.Password, employee.UserName);
                if (registerValue == 1)
                {
                    ViewBag.Message = $"The Emplyee {employee.Name} is signed up successfully";
                }
            }
            ViewBag.Role = new SelectList(db.s_Role, "RoleId", "RoleName");
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(EmployeeLogin employee)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser(employee))
                {
                    FormsAuthentication.SetAuthCookie(employee.UserName, false);
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("UserNamePassword", "UserName Password does not match");
                    return View("Login");
                }
            }
            return View("Login");
        }

        [NonAction]
        private bool ValidateUser(EmployeeLogin employee)
        {
            bool isValid = false;
            int? userId = db.sp_ValidateEmployee(employee.UserName, employee.Password).FirstOrDefault();

            if(userId == null)
            {
                throw new Exception("User ID has null value. Some error occured");
            }
            else if (userId == -1)
            {
                isValid = false;
            }
            else
            {
                s_Employee emp = db.s_Employee.Single(x => x.UserID == userId);
                Session["UserName"] = employee.UserName;
                Session["Role"] = db.s_Employee.Single(x => x.UserID == userId).Role;
                isValid = true;
            }
            return isValid;
        }
        
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}