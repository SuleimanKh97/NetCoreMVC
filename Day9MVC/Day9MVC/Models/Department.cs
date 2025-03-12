using System.ComponentModel.DataAnnotations;

namespace HRManagement.Models
{
    public class Department
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string ManagerId { get; set; }
        public ApplicationUser Manager { get; set; }

        public virtual ICollection<ApplicationUser> Employees { get; set; }
    }
} 