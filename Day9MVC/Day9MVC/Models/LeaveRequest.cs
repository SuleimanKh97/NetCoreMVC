using System.ComponentModel.DataAnnotations;

namespace HRManagement.Models
{
    public class LeaveRequest
    {
        public string Id { get; set; }

        public string EmployeeId { get; set; }
        public ApplicationUser Employee { get; set; }

        [Required]
        public string Reason { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public LeaveStatus Status { get; set; }

        public string? ApprovedById { get; set; }
        public ApplicationUser ApprovedBy { get; set; }

        public DateTime RequestDate { get; set; }
    }

    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }
} 