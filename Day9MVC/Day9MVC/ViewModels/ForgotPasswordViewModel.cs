using System.ComponentModel.DataAnnotations;

namespace HRManagement.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
} 