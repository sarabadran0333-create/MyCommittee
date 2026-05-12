using System.ComponentModel.DataAnnotations;

namespace MyCommittee.Models
{
    public class CommitteeType
    {
        [Key]
        public int TypeId { get; set; }
        public string? Name { get; set; }
    }
}
