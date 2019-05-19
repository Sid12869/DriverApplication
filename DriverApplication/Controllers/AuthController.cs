using DriverApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using DriverApplication.CustomLiberaries;
using System.Net;

namespace DriverApplication.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {              
            return View();
        }

        [HttpPost]
        public ActionResult Login(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Email == "sid@admin.com" && model.Password == "34567")
            {
                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, "Sid"),
                    new Claim(ClaimTypes.Email, "Sid@email.com"),
                    new Claim(ClaimTypes.Country, "England")
                    },
                   "ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                authManager.SignIn(identity);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("1001", "Invalid email or password");
            return View(model);
        }

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(User model) {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult((HttpStatusCode) 422, "Invalid Details Found");
            }
            using (var db = new MainDbContext())
            {
                var encryptedPassword = Encryptor.Encrypt(model.Password, model.Email);
                var user = db.Users.Create();
                user.Email = model.Email;
                user.Password = encryptedPassword;
                user.Country = model.Country;
                user.Name = model.Name;
                db.Users.Add(user);
                db.SaveChanges();
            }
            return View();
        }
    }
}