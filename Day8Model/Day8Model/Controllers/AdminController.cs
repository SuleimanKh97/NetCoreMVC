using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Day8Model.Models;

namespace Day8Model.Controllers
{
    public class AdminController : Controller
    {
        private readonly MyDbContext _context;

        public AdminController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            return View(await _context.User1s.ToListAsync());
        }

      

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user2 = await _context.User1s
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user2 == null)
            {
                return NotFound();
            }

            return View(user2);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Password,Email")] User2 user2)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user2);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user2);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user2 = await _context.User1s.FindAsync(id);
            if (user2 == null)
            {
                return NotFound();
            }
            return View(user2);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Password,Email")] User2 user2)
        {
            if (id != user2.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user2);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!User2Exists(user2.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user2);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user2 = await _context.User1s
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user2 == null)
            {
                return NotFound();
            }

            return View(user2);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user2 = await _context.User1s.FindAsync(id);
            if (user2 != null)
            {
                _context.User1s.Remove(user2);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool User2Exists(int id)
        {
            return _context.User1s.Any(e => e.Id == id);
        }





        public async Task<IActionResult> IndexProduct()
        {
            return View(await _context.Products.ToListAsync());
        }



    }
}
