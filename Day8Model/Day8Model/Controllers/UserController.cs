using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Day8Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Day8Model.Controllers
{
    public class UserController : Controller
    {
        private readonly MyDbContext DB;

        public UserController(MyDbContext db)
        {
            DB = db;
        }
        // GET: UserController
        public ActionResult Index()
        {
            var products = DB.Products.ToList();
            return View(products);
        }

        // GET: UserController/Details/5
        public ActionResult Details()
        {
            var userloged = HttpContext.Session.GetInt32("IDuser");
            var userdetails = DB.User2s.Find(userloged);
            return View(userdetails);
        }



        public IActionResult EditProfile(int? id)
        {
            var userloged = HttpContext.Session.GetInt32("IDuser");

            var profile = DB.User2s.Find(userloged);

            return View(profile);
        }


        [HttpPost]
        public IActionResult EditProfile(User2 user)
        {




            DB.Update(user);
            DB.SaveChanges();


            return RedirectToAction(nameof(Details));


        }

        // GET: UserController/Create
        public ActionResult regiester()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult regiester(User2 user)
        {
            try
            {
                DB.User1s.Add(user);
                DB.SaveChanges();

                return RedirectToAction(nameof(Login));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Login(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        public ActionResult Login(int id, User2 user)
        {
            try
            {
                var User2 = DB.User2s.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
                if (user.Email=="admin@admin.com" && user.Password=="admin")
                {
                    return RedirectToAction("Index" , "Admin");
                }
                if (User2 != null)
                {

                    HttpContext.Session.SetInt32("IDuser", User2.Id);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
