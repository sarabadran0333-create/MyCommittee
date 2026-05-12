using System.ComponentModel.DataAnnotations;

namespace MyCommittee.Models
{
    public class MinutesOfMeeting
    {
        [Key]
        public int MinutesOfMeetingId { get; set; }
        public int CalendarId { get; set; }
        public string? Minutes { get; set; }
    }
}
