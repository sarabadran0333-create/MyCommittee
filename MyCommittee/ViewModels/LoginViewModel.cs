using System.ComponentModel.DataAnnotations;

namespace MyCommittee.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? Role { get; set; }
       
    }
}