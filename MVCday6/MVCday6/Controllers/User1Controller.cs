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
    public class User1Controller : Controller
    {
        private readonly MyDbContext _context;

        public User1Controller(MyDbContext context)
        {
            _context = context;
        }

        // GET: User1
        public async Task<IActionResult> Index()
        {
            return View(await _context.User1s.ToListAsync());
        }

       

        // GET: User1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User1/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email")] User1 user1)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user1);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user1);
        }

       

        

        
        
    }
}
