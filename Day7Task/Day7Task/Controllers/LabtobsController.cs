using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Day7Task.Models;

namespace Day7Task.Controllers
{
    public class LabtobsController : Controller
    {
        private readonly MyDbContext _context;

        public LabtobsController(MyDbContext context)
        {
            _context = context;
        }

        
        public  IActionResult Index()
        {
            return View( _context.Labtobs.ToList());
        }

        
        public  IActionResult Details(int? id)
        {
            

            var labtob =  _context.Labtobs
                .FirstOrDefault(m => m.Id == id);
            

            return View(labtob);
        }

        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        public  IActionResult Create(Labtob labtob)
        {
           
                _context.Add(labtob);
                 _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            
            return View(labtob);
        }

        public  IActionResult Edit(int? id)
        {
            
            var labtob =  _context.Labtobs.Find(id);
            
            return View(labtob);
        }

        
        [HttpPost]
        public IActionResult Edit(int id, Labtob labtob)
        {
            

           
                
                    _context.Update(labtob);
                     _context.SaveChanges();
                
               
                return RedirectToAction(nameof(Index));
            
          
        }

        // GET: Labtobs/Delete/5
        public IActionResult Delete(int? id)
        {
           

            var labtob =  _context.Labtobs
                .FirstOrDefault(m => m.Id == id);
           

            return View(labtob);
        }

        // POST: Labtobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var labtob = await _context.Labtobs.FindAsync(id);
            if (labtob != null)
            {
                _context.Labtobs.Remove(labtob);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabtobExists(int id)
        {
            return _context.Labtobs.Any(e => e.Id == id);
        }
    }
}
