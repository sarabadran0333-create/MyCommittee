using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace MyCommittee.ViewModels
{
    public class AdminDashboardViewModel
    {
        // Add this property to fix CS1061
        public int MyCommittees { get; set; }

        // If you have other properties referenced in Dashboard.cshtml, add them as well:
        public int CommitteeMembers { get; set; }
        public int UpcomingMeetings { get; set; }
        public int PendingMinutes { get; set; }
    }
}