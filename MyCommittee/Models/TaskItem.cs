using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCommittee.Models
{
    public class TaskItem
    {
        [Key]
        public int TaskId { get; set; }

        public string Task { get; set; }

        public DateTime Deadline { get; set; }

        public bool IsSubmitted { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public int AssignedToJobId { get; set; }

        public int CommitteeId { get; set; }

        public int MinutesId { get; set; }

        [ForeignKey("AssignedToJobId")]
        public Member Member { get; set; }

        [ForeignKey("CommitteeId")]
        public Committee Committee { get; set; }

        [ForeignKey("MinutesId")]
        public MinutesOfMeeting MinutesOfMeeting { get; set; }
    }
}