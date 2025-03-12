using System.ComponentModel.DataAnnotations;

namespace HRManagement.ViewModels
{
    public class AssignTaskViewModel
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Required]
        [Display(Name = "Assign To")]
        public string AssignedToId { get; set; }
    }
} 