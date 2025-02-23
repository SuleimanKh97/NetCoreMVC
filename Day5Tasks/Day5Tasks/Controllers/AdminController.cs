using Microsoft.AspNetCore.Mvc;

namespace Day5Tasks.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Admin()
        {
            return View();
        }
    }
}
