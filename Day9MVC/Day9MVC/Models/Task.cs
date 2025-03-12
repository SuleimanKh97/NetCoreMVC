using System.ComponentModel.DataAnnotations;

namespace HRManagement.Models
{
    public class Task
    {
        public string Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public TaskStatus Status { get; set; }

        public string AssignedToId { get; set; }
        public ApplicationUser AssignedTo { get; set; }

        public string AssignedById { get; set; }
        public ApplicationUser AssignedBy { get; set; }
    }

    public enum TaskStatus
    {
        ToDo,
        Doing,
        Done
    }
} 