using System.ComponentModel.DataAnnotations;

namespace MyCommittee.Models
{
    public class Decision
    {
        [Key]
        public int DecisionId { get; set; }
        public int CalendarId { get; set; }
        public string? Notes { get; set; }


    }
}
