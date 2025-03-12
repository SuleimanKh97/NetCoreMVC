using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HRManagement.Data;
using HRManagement.Models;
using HRManagement.ViewModels;

namespace HRManagement.Controllers
{
    [Authorize(Roles = "HR")]
    public class HRController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HRController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.ToListAsync();
            return View(employees);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Department,Position,Salary,HireDate")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Department,Position,Salary,HireDate")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            return View(employee);
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Departments()
        {
            var departments = await _context.Departments
                .Include(d => d.Manager)
                .ToListAsync();
            return View(departments);
        }

        [HttpGet]
        public IActionResult CreateDepartment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var department = new Department
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Description = model.Description
                };

                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Departments));
            }
            return View(model);
        }

        public async Task<IActionResult> Managers()
        {
            var managers = await _userManager.GetUsersInRoleAsync("Manager");
            return View(managers);
        }

        [HttpGet]
        public async Task<IActionResult> CreateManager()
        {
            ViewBag.Departments = await _context.Departments
                .Where(d => d.ManagerId == null)
                .ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateManager(CreateManagerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DepartmentId = model.DepartmentId,
                    Position = "Manager",
                    JoinDate = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Manager");
                    
                    var department = await _context.Departments.FindAsync(model.DepartmentId);
                    if (department != null)
                    {
                        department.ManagerId = user.Id;
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Managers));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewBag.Departments = await _context.Departments
                .Where(d => d.ManagerId == null)
                .ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Feedbacks()
        {
            var feedbacks = await _context.Feedbacks
                .OrderByDescending(f => f.SubmissionDate)
                .ToListAsync();
            return View(feedbacks);
        }
    }
} 