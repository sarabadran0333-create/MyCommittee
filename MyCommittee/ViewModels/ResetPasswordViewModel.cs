using System.ComponentModel.DataAnnotations;

namespace MyCommittee.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "A new password is required.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The passwords do not match.")] // هذا السطر يحل المشكلة الثانية تلقائياً
        public string ConfirmPassword { get; set; }
    }
}