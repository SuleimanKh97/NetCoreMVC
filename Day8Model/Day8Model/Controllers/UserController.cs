using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Day8Model.Models;

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
            
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult regiester()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult regiester(User1 user)
        {
            try
            {
                DB.User1s.Add(user);
                DB.SaveChanges();

                return RedirectToAction(nameof(Index));
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
        public ActionResult Login(int id, User1 user)
        {
            try
            {
                var user1 = DB.User1s.FirstOrDefault(x => x.Email == user.Email && x.Name == user.Name);
                if (user1 != null)
                {
                    TempData["UserName"] = user.Name;

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
