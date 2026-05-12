using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCommittee.Models
{
    public class Committee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommitteeId { get; set; }
        public string? Title { get; set; }= null;
        public int TypeId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }



    }
}
