using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace WEDTask.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginProsses(string email, string password)
        {
            string sessionEmail = HttpContext.Session.GetString("email");
            string sessionPassword = HttpContext.Session.GetString("password");
            if (email == sessionEmail && password == sessionPassword)
            {
                return RedirectToAction("Index" , "Home");
            }
            else
            {
                TempData["msg"] = "Invalid Email or Password";
                return RedirectToAction("Login");
            }

            
        }


        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterProsses(string username, string email, string password, string rpassword)
        {
            HttpContext.Session.SetString("username", username);
            HttpContext.Session.SetString("email", email);
            HttpContext.Session.SetString("password", password);
            HttpContext.Session.SetString("rpassword", rpassword);

            if (password != rpassword)
            {
                TempData["msg"] = "Password and Confirm Password must be same";
                return RedirectToAction("Register");
            }

            return RedirectToAction("Login");




        }



        public IActionResult Profile()
        {
            string name = HttpContext.Session.GetString("username");
            string email = HttpContext.Session.GetString("email");
            string password = HttpContext.Session.GetString("password");

            ViewData["name"] = name;
            ViewData["email"] = email;
            ViewData["password"] = password;
            return View();
        }




    }
}
