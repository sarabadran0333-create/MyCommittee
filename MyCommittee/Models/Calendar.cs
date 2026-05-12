using System.ComponentModel.DataAnnotations;

namespace MyCommittee.Models
{
    public class Calendar
    {
        [Key]
    public int CalendarId { get; set; }
    public int CommitteeId { get; set; }
    public DateTime MeetingTime { get; set; }
    public string? Place { get; set; }
    public string? MeetingTitle { get; set; }


    }
}
