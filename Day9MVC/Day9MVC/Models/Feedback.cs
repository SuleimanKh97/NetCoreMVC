using System.ComponentModel.DataAnnotations;

namespace HRManagement.Models
{
    public class Feedback
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime SubmissionDate { get; set; }
    }
} 