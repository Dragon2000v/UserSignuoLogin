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
        public ActionResult Index(string sortField, string sortOrder)
        {
            var model = db.UserInfs.ToList();
            ViewBag.SortField = sortField;
            ViewBag.SortOrder = sortOrder;

            // Определите направление сортировки
            bool isDescending = sortOrder == "desc";

            // Выполните сортировку в зависимости от параметра sortField и направления сортировки
            switch (sortField)
            {
                case "full_name":
                    model = isDescending
                        ? model.OrderByDescending(item => item.full_name).ToList()
                        : model.OrderBy(item => item.full_name).ToList();
                    break;
                case "age":
                    model = isDescending
                        ? model.OrderByDescending(item => item.age).ToList()
                        : model.OrderBy(item => item.age).ToList();
                    break;
                case "regional_center":
                    model = isDescending
                        ? model.OrderByDescending(item => item.regional_center).ToList()
                        : model.OrderBy(item => item.regional_center).ToList();
                    break;
                default:
                    // Если sortField не указан или не соответствует ни одному полю, оставьте данные без сортировки
                    break;
            }

            return View(model);
        }



        public ActionResult Signup()
        {
            var cities = new List<string>
            {
                "Kyiv",
                "Lviv",
                "Kharkiv",
                "Odesa",
                "Dnipro"
            };

            ViewBag.CityList = new SelectList(cities);
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
                
               // userInf.AvailableCities = GetUkraineCities();

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