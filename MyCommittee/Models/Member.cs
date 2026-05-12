using System.ComponentModel.DataAnnotations;

namespace MyCommittee.Models
{
    public class Member
    {
        [Key]
        public int JobId { get; set; }
        public string? Username { get; set; }
        public string? EMail { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public int RoleId { get; set; }

    }
}