using System.ComponentModel.DataAnnotations;

namespace HRManagement.ViewModels
{
    public class DepartmentViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
} 