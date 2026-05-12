using System.ComponentModel.DataAnnotations;

namespace MyCommittee.Models
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int CommitteeId { get; set; }
    }
}