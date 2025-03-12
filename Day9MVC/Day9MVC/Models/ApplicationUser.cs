using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HRManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        public string ProfileImage { get; set; }

        [Required]
        public string DepartmentId { get; set; }
        public Department Department { get; set; }

        [Required]
        public string Position { get; set; }

        public DateTime JoinDate { get; set; }

        // Navigation properties
        public virtual ICollection<Task> AssignedTasks { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }
    }
} 