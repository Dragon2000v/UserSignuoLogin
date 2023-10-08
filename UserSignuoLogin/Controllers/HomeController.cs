using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserSignuoLogin.Models;

namespace UserSignuoLogin.Controllers
{
    public class HomeController : Controller
    {
        DBuserSingupLoginEntities db = new DBuserSingupLoginEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View(db.UserInfs.ToList());
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(UserInf userInf)
        {
            if (db.UserInfs.Any(x=>x.email == userInf.email))
            {
                ViewBag.Notification = "This account has already existed";
                return View();
            }
            else
            {
                db.UserInfs.Add(userInf);
                db.SaveChanges();

                Session["id"] = userInf.id.ToString();
                Session["email"] = userInf.email.ToString();
                return RedirectToAction("Index", "Home");
            }
            
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserInf userInf)
        {
            var chekLogin = db.UserInfs.Where(x => x.email.Equals(userInf.email) && x.password.Equals(userInf.password)).FirstOrDefault();
            if(chekLogin != null)
            {
                Session["id"] = userInf.id.ToString();
                Session["email"] = userInf.email.ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Notification = "Wrong Username or password";
            }
            return View();
        }
    }
}