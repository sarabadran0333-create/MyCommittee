using System.ComponentModel.DataAnnotations;

namespace MyCommittee.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }
        public int JobId { get; set; }
        public bool? Attend { get; set; }
        public int CalendarId { get; set; }
    }
}