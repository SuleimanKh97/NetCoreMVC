using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.AspNetCore.Http;



namespace Day5Tasks.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Login()
        {
            
                return View();
        }
        [HttpPost]
        public IActionResult LoginProsses(string email, string password , string rem)
        {
            string sessionEmail = HttpContext.Session.GetString("email");
            string sessionPassword = HttpContext.Session.GetString("password");
            if (email == sessionEmail && password == sessionPassword)
            {
                if (rem == "yes")
                {
                    CookieOptions obj = new CookieOptions();
                    obj.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Append("userInfo", email, obj);
                }
                return RedirectToAction("Index" , "Home");
            }
            else if (email == "admin@gmail.com" && password == "admin")
            {
                return RedirectToAction("Admin", "Admin");
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

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("password");
            return RedirectToAction("Login");
        }

        public IActionResult EditProfile()
        {
            return View();

        }

        public IActionResult EditProfileProsses(string phone, string email, string address, string password , string name)
        {
            HttpContext.Session.SetString("username", name);
            HttpContext.Session.SetString("email", email);
            HttpContext.Session.SetString("password", password);
            HttpContext.Session.SetString("addresss", address);
            HttpContext.Session.SetString("phone", phone);

            ViewData["name"] = name;
            ViewData["email"] = email;
            ViewData["password"] = password;
            ViewData["addresss"] = address;
            ViewData["phone"] = phone;
            return RedirectToAction("Profile");

        }



    }
}
