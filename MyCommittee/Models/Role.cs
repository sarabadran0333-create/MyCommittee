using System.ComponentModel.DataAnnotations;

namespace MyCommittee.Models
{
    public class Role
    { 
    [Key]
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
    }
}