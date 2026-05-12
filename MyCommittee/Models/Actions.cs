using System.ComponentModel.DataAnnotations;

namespace MyCommittee.Models
{
    public class Actions
    {
        [Key]
        public int ActivityId { get; set; }
        public int JobId { get; set; }
        public string? ActionType { get; set; }
        public string? Details { get; set; }
        public DateTime Date { get; set; }
        public int CommitteeId { get; set; }
    }
}