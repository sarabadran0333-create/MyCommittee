using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCommittee.Data;
using MyCommittee.Models;
using MyCommittee.ViewModels;
using System.Linq;


public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ================= Dashboard =================
    public IActionResult Dashboard()
    {
        int totalCommitteeCount = _context.Committees.Count();
        ViewBag.TotalCommittees = totalCommitteeCount;

        int totalMemberCount = _context.Members.Count();
        ViewBag.TotalMembers = totalMemberCount;


        var upcomingMeetings = _context.Calendars.Where(m => m.MeetingTime > DateTime.Now).OrderBy(m => m.MeetingTime).Take(3).ToList();

        int totalUpcomingMeetingCount = upcomingMeetings.Count();
        ViewBag.TotalUpcomingMeetings = totalUpcomingMeetingCount;

        int totalMinutesCount = _context.MinutesOfMeetings.Count();
        ViewBag.TotalMinutes = totalMinutesCount;

        var recentActivities = _context.Actions
            .Include(a => a.Member)
            .Include(a => a.Committee)
         .OrderByDescending(a => a.Date) // الأحدث أولاً
         .Take(5) // عرض آخر 5 أنشطة
         .ToList();

        ViewBag.RecentActivities = recentActivities;


        // جلب اللجان مع عد أعضائها من جدول الربط
        var committeeMembership = _context.Committees
            .Select(c => new {
                CommitteeName = c.Title,
                // هنا نعد كم مرة ظهر رقم اللجنة في جدول CommitteeMembers
                MemberCount = _context.CommitteeMembers.Count(cm => cm.CommitteeId == c.CommitteeId)

            })
            .OrderByDescending(c => c.MemberCount)
            .Take(3)
            .ToList();

        ViewBag.CommitteesMembership = committeeMembership;

        return View(upcomingMeetings);
    }

    // ================= Committees =================
    public IActionResult Committees()
    {
        var types = _context.CommitteeTypes.ToList();

        ViewBag.CommitteeTypes = types;

        ViewBag.Leaders = _context.Members
            .Where(x => x.RoleId == 2)
            .ToList();

        ViewBag.Members = _context.Members
      .Where(x => x.RoleId == 3)
      .ToList();

        var data = _context.Committees
            .Select(c => new
            {
                c.CommitteeId,
                c.Title,
                c.TypeId,
                c.Start,
                c.End,
                Permissions = _context.Permissions
            .Where(p => p.CommitteeId == c.CommitteeId)
            .Select(p => p.Title)
            .ToList(),
                Descriptions = _context.Permissions
    .Where(p => p.CommitteeId == c.CommitteeId)
    .Select(p => p.Description)
    .ToList(),
                MemberIds = _context.CommitteeMembers
    .Where(cm =>
        cm.CommitteeId == c.CommitteeId &&
        _context.Members
            .FirstOrDefault(m => m.JobId == cm.JobId).RoleId == 3
    )
    .Select(cm => cm.JobId)
    .ToList(),
                TypeName = _context.CommitteeTypes
                    .Where(t => t.TypeId == c.TypeId)
                    .Select(t => t.Name)
                    .FirstOrDefault(),
                Leader = _context.CommitteeMembers
    .Join(_context.Members,
        cm => cm.JobId,
        m => m.JobId,
        (cm, m) => new { cm, m })
    .Where(x =>
        x.cm.CommitteeId == c.CommitteeId &&
        x.m.RoleId == 2)
    .Select(x => x.m.Username)
    .FirstOrDefault(),

                LeaderId = _context.CommitteeMembers
    .Join(_context.Members,
        cm => cm.JobId,
        m => m.JobId,
        (cm, m) => new { cm, m })
    .Where(x =>
        x.cm.CommitteeId == c.CommitteeId &&
        x.m.RoleId == 2)
    .Select(x => x.m.JobId)
    .FirstOrDefault(),

                MembersCount = _context.CommitteeMembers
                    .Count(cm => cm.CommitteeId == c.CommitteeId)
            })
            .ToList();

        return View(data);
    }

    // ================= Pages =================
    public IActionResult Meetings()
    {
        var meetingsList = _context.Calendars
                                .Include(m => m.Committee)
                                .Include(c => c.MinutesOfMeeting)
                                .Where(c => c.MeetingTime < DateTime.Now)
                                .OrderByDescending(c => c.MeetingTime)
                                .ToList();
        return View(meetingsList);
    }

    public IActionResult Minutes()
    {
        return View();
    }

    public IActionResult Notifications()
    {
        return View();
    }

    public IActionResult Settings()
    {
        return View();
    }

    public IActionResult Profile()
    {
        return View();
    }

    // ================= Create Committee =================
    public IActionResult CreateCommittee()
    {
        ViewBag.CommitteeTypes = _context.CommitteeTypes.ToList();

        return View();
    }

    // ================= Save Committee =================
    [HttpPost]
    public IActionResult SaveCommittee(
        string Title,
        int TypeId,
        int? Leader,
        List<int> Members,
        DateTime Start,
        DateTime End,
        List<string> Permissions,
        List<string> Descriptions)
    {
        var committee = new Committee
        {
            Title = Title,
            TypeId = TypeId,
            Start = Start,
            End = End
        };

        _context.Committees.Add(committee);

        _context.SaveChanges();

        // ================= Leader =================
        if (Leader.HasValue)
        {
            _context.CommitteeMembers.Add(new CommitteeMembers
            {
                CommitteeId = committee.CommitteeId,
                JobId = Leader.Value
            });
        }

        // ================= Members =================
        if (Members != null)
        {
            foreach (var memberId in Members)
            {
                if (Leader.HasValue && memberId == Leader.Value)
                    continue;

                _context.CommitteeMembers.Add(new CommitteeMembers
                {
                    CommitteeId = committee.CommitteeId,
                    JobId = memberId
                });
            }
        }

        // ================= Permissions =================
        if (Permissions != null)
        {
            for (int i = 0; i < Permissions.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(Permissions[i]))
                {
                    _context.Permissions.Add(new Permission
                    {
                        CommitteeId = committee.CommitteeId,
                        Title = Permissions[i],

                        Description = Descriptions.Count > i
                            ? Descriptions[i]
                            : null
                    });
                }
            }
        }
        _context.SaveChanges();

        return RedirectToAction("Committees");
    }

    // ================= Update Committee =================
    [HttpPost]
    public IActionResult UpdateCommittee(
    int CommitteeId,
    string Title,
    int TypeId,
    int? Leader,
    DateTime Start,
    DateTime End,
    List<int> Members,
    List<string> Permissions,
    List<string> Descriptions)
    {
        var committee = _context.Committees.Find(CommitteeId);

        if (committee == null)
            return NotFound();

        committee.Title = Title;
        committee.TypeId = TypeId;
        committee.Start = Start;
        committee.End = End;

        _context.SaveChanges();

        // حذف القديم
        var oldMembers = _context.CommitteeMembers
            .Where(x => x.CommitteeId == CommitteeId);

        _context.CommitteeMembers.RemoveRange(oldMembers);

        _context.SaveChanges();
        var oldPermissions = _context.Permissions
    .Where(x => x.CommitteeId == CommitteeId);

        _context.Permissions.RemoveRange(oldPermissions);

        _context.SaveChanges();

        // Leader

        if (Leader.HasValue)
        {
            _context.CommitteeMembers.Add(new CommitteeMembers
            {
                CommitteeId = CommitteeId,
                JobId = Leader.Value
            });
        }
        // Members
        if (Members != null)
        {
            foreach (var memberId in Members)
            {
                if (Leader.HasValue && memberId == Leader.Value)
                    continue;

                _context.CommitteeMembers.Add(new CommitteeMembers
                {
                    CommitteeId = CommitteeId,
                    JobId = memberId
                });
            }
        }
        if (Permissions != null)
        {

            for (int i = 0; i < Permissions.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(Permissions[i]))
                {
                    _context.Permissions.Add(new Permission
                    {
                        CommitteeId = CommitteeId,
                        Title = Permissions[i],

                        Description = Descriptions.Count > i
                            ? Descriptions[i]
                            : null
                    });
                }
            }
        }
        _context.SaveChanges();

        return RedirectToAction("Committees");
    }

    // ================= Delete Committee =================
    [HttpPost]
    public IActionResult DeleteCommittee(int id)
    {
        var committee = _context.Committees
            .FirstOrDefault(x => x.CommitteeId == id);

        if (committee == null)
            return Ok();

        // ================= Calendars =================
        var calendars = _context.Calendars
            .Where(x => x.CommitteeId == id)
            .ToList();

        foreach (var calendar in calendars)
        {
            // Attendances
            var attendances = _context.Attendances
                .Where(x => x.CalendarId == calendar.CalendarId);

            _context.Attendances.RemoveRange(attendances);

            // Decisions
            var decisions = _context.Decisions
                .Where(x => x.CalendarId == calendar.CalendarId);

            _context.Decisions.RemoveRange(decisions);

            // Minutes
            var minutes = _context.MinutesOfMeetings
                .Where(x => x.CalendarId == calendar.CalendarId);

            _context.MinutesOfMeetings.RemoveRange(minutes);
        }

        // Save أول
        _context.SaveChanges();

        // ================= Calendars =================
        _context.Calendars.RemoveRange(calendars);

        // ================= Committee Members =================
        var committeeMembers = _context.CommitteeMembers
            .Where(x => x.CommitteeId == id);

        _context.CommitteeMembers.RemoveRange(committeeMembers);

        // ================= Permissions =================
        var permissions = _context.Permissions
            .Where(x => x.CommitteeId == id);

        _context.Permissions.RemoveRange(permissions);

        // ================= Actions =================
        var actions = _context.Actions
            .Where(x => x.CommitteeId == id);

        _context.Actions.RemoveRange(actions);

        _context.SaveChanges();

        // ================= Delete Committee =================
        _context.Committees.Remove(committee);

        _context.SaveChanges();

        return Ok();
    }

    // هذا الـ Action هو المسؤول عن استقبال طلب الطباعة
    public async Task<IActionResult> PrintMinutes(int id)
    {
        // بيانات الاجتماع مع اللجنة ومع المحضر المرتبط به
        var meeting = await _context.Calendars
            .Include(c => c.Committee)
            .Include(c => c.MinutesOfMeeting) // هذا مهم جداً لجلب نص المحضر
            .FirstOrDefaultAsync(m => m.CalendarId == id);

        if (meeting == null)
        {
            return NotFound();
        }
        return View(meeting);
    }
}