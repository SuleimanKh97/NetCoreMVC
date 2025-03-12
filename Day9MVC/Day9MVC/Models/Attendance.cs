using System.ComponentModel.DataAnnotations;

namespace HRManagement.Models
{
    public class Attendance
    {
        public string Id { get; set; }

        public string EmployeeId { get; set; }
        public ApplicationUser Employee { get; set; }

        public DateTime Date { get; set; }
        public DateTime? PunchIn { get; set; }
        public DateTime? PunchOut { get; set; }
    }
} 