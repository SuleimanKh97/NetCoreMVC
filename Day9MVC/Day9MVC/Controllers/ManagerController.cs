using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HRManagement.Data;
using HRManagement.Models;
using HRManagement.ViewModels;

namespace HRManagement.Controllers
{
    public class ManagerController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManagerController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = GetCurrentUserId();
            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.ManagerId == currentUserId);

            var dashboardViewModel = new ManagerDashboardViewModel
            {
                TotalEmployees = department?.Employees.Count ?? 0,
                PendingTasks = await _context.Tasks
                    .Where(t => t.AssignedById == currentUserId && t.Status == TaskStatus.ToDo)
                    .CountAsync(),
                PendingLeaveRequests = await _context.LeaveRequests
                    .Where(l => l.Status == LeaveStatus.Pending && 
                           l.Employee.DepartmentId == department.Id)
                    .CountAsync()
            };

            return View(dashboardViewModel);
        }

        public async Task<IActionResult> Employees()
        {
            var currentUserId = GetCurrentUserId();
            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.ManagerId == currentUserId);

            return View(department?.Employees ?? new List<ApplicationUser>());
        }

        [HttpGet]
        public IActionResult CreateEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = GetCurrentUserId();
                var department = await _context.Departments
                    .FirstOrDefaultAsync(d => d.ManagerId == currentUserId);

                if (department == null)
                {
                    return NotFound();
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DepartmentId = department.Id,
                    Position = model.Position,
                    JoinDate = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employee");
                    return RedirectToAction(nameof(Employees));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> EmployeeTasks(string id)
        {
            var tasks = await _context.Tasks
                .Where(t => t.AssignedToId == id)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();

            ViewBag.EmployeeName = (await _userManager.FindByIdAsync(id))?.FirstName;
            return View(tasks);
        }

        [HttpGet]
        public async Task<IActionResult> AssignTask()
        {
            var currentUserId = GetCurrentUserId();
            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.ManagerId == currentUserId);

            ViewBag.Employees = new SelectList(department?.Employees ?? 
                new List<ApplicationUser>(), "Id", "FirstName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignTask(AssignTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var task = new Task
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = model.Title,
                    Description = model.Description,
                    DueDate = model.DueDate,
                    CreatedDate = DateTime.Now,
                    Status = TaskStatus.ToDo,
                    AssignedToId = model.AssignedToId,
                    AssignedById = GetCurrentUserId()
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                // TODO: Send email notification to employee

                return RedirectToAction(nameof(Index));
            }

            var currentUserId = GetCurrentUserId();
            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.ManagerId == currentUserId);

            ViewBag.Employees = new SelectList(department?.Employees ?? 
                new List<ApplicationUser>(), "Id", "FirstName");
            return View(model);
        }

        public async Task<IActionResult> LeaveRequests()
        {
            var currentUserId = GetCurrentUserId();
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.ManagerId == currentUserId);

            var requests = await _context.LeaveRequests
                .Include(l => l.Employee)
                .Where(l => l.Employee.DepartmentId == department.Id)
                .OrderByDescending(l => l.RequestDate)
                .ToListAsync();

            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveLeave(string id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            leaveRequest.Status = LeaveStatus.Approved;
            leaveRequest.ApprovedById = GetCurrentUserId();
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(LeaveRequests));
        }

        [HttpPost]
        public async Task<IActionResult> RejectLeave(string id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            leaveRequest.Status = LeaveStatus.Rejected;
            leaveRequest.ApprovedById = GetCurrentUserId();
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(LeaveRequests));
        }
    }
} 