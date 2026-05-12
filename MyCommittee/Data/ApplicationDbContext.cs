using Microsoft.EntityFrameworkCore;
using MyCommittee.Models;
using System.Security;

namespace MyCommittee.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Committee> Committees { get; set; } = null!;
        public DbSet<CommitteeMembers> CommitteeMembers { get; set; } = null!;
        public DbSet<Calendar> Calendars { get; set; } = null!;
        public DbSet<Attendance> Attendances { get; set; } = null!;
        public DbSet<MinutesOfMeeting> MinutesOfMeetings { get; set; } = null!;
        public DbSet<Decision> Decisions { get; set; } = null!;
        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<CommitteeType> CommitteeTypes { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Actions> Actions { get; set; }
        public IEnumerable<object> Attendance { get; internal set; }
    }
}
