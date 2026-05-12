using System.ComponentModel.DataAnnotations;


namespace MyCommittee.Models
{
    public class AdminDashboardViewModel
    {
       
        public int MyCommittees { get; set; }

   
        public int CommitteeMembers { get; set; }
        public int UpcomingMeetings { get; set; }
        public int PendingMinutes { get; set; }
    }
}