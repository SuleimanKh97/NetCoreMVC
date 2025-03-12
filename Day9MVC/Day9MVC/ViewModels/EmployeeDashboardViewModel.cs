using HRManagement.Models;

namespace HRManagement.ViewModels
{
    public class EmployeeDashboardViewModel
    {
        public IEnumerable<Task> Tasks { get; set; }
        public IEnumerable<Attendance> RecentAttendances { get; set; }
        public IEnumerable<LeaveRequest> LeaveRequests { get; set; }
    }
} 