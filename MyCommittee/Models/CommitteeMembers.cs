using System.ComponentModel.DataAnnotations;

namespace MyCommittee.Models
{
    
        public class CommitteeMembers
        {
            [Key]
            public int CommitteeMembersId { get; set; }  

            public int CommitteeId { get; set; }

            public int JobId { get; set; }
        }
    }


