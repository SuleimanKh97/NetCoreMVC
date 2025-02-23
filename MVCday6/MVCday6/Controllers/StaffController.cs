using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCday6.Models;

namespace MVCday6.Controllers
{
    public class StaffController : Controller
    {
        private readonly MyDbContext _context;

        public StaffController(MyDbContext context)
        {
            _context = context;
        }





        // GET: Students
        public IActionResult Index()
        {
            return View(_context.Staff.ToList());
        }
      


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]

        public IActionResult Create(Staff staff)
        {

            _context.Add(staff);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
