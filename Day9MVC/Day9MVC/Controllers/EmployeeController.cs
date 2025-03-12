using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HRManagement.Data;
using HRManagement.Models;
using HRManagement.ViewModels;

namespace HRManagement.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = GetCurrentUserId();
            var dashboardViewModel = new EmployeeDashboardViewModel
            {
                Tasks = await _context.Tasks
                    .Where(t => t.AssignedToId == currentUserId)
                    .OrderBy(t => t.Status)
                    .ThenBy(t => t.DueDate)
                    .ToListAsync(),
                RecentAttendances = await _context.Attendances
                    .Where(a => a.EmployeeId == currentUserId)
                    .OrderByDescending(a => a.Date)
                    .Take(5)
                    .ToListAsync(),
                LeaveRequests = await _context.LeaveRequests
                    .Where(l => l.EmployeeId == currentUserId)
                    .OrderByDescending(l => l.RequestDate)
                    .Take(5)
                    .ToListAsync()
            };

            return View(dashboardViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PunchIn()
        {
            var currentUserId = GetCurrentUserId();
            var today = DateTime.Today;
            
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.EmployeeId == currentUserId && a.Date == today);

            if (attendance == null)
            {
                attendance = new Attendance
                {
                    Id = Guid.NewGuid().ToString(),
                    EmployeeId = currentUserId,
                    Date = today,
                    PunchIn = DateTime.Now
                };
                _context.Attendances.Add(attendance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> PunchOut()
        {
            var currentUserId = GetCurrentUserId();
            var today = DateTime.Today;
            
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.EmployeeId == currentUserId && a.Date == today);

            if (attendance != null)
            {
                attendance.PunchOut = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult RequestLeave()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RequestLeave(LeaveRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var leaveRequest = new LeaveRequest
                {
                    Id = Guid.NewGuid().ToString(),
                    EmployeeId = GetCurrentUserId(),
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Reason = model.Reason,
                    Status = LeaveStatus.Pending,
                    RequestDate = DateTime.Now
                };

                _context.LeaveRequests.Add(leaveRequest);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTaskStatus(string taskId, TaskStatus newStatus)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task != null && task.AssignedToId == GetCurrentUserId())
            {
                task.Status = newStatus;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
} 