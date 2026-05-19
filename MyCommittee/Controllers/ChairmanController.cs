using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCommittee.Data;
using MyCommittee.Models;

namespace MyCommittee.Controllers
{
    public class ChairmanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private DateTime currentTime;

        public ChairmanController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int committeesCount = _context.CommitteeMembers
                                  .Count(cm => cm.JobId == userId);
            ViewBag.CommitteesCount = committeesCount;

            //تحديد اللجان اللي هاد اليوزر هو الليدر عليها
            //--------------------------------------------------------
            var chairmanCommitteeIds = _context.CommitteeMembers
                                       .Where(c => c.JobId == userId)
                                       .Select(c => c.CommitteeId)
                                       .ToList();
            //----------------------------------------------------
            var chairmanCommittees = _context.Committees
                                     .Where(c => chairmanCommitteeIds.Contains(c.CommitteeId)) 
                                     .ToList();

            ViewBag.MyCommitteesList = chairmanCommittees;

            var recentActivities = _context.Actions
                               .Include(a => a.Committee) // جلب بيانات اللجنة المرتبطة بـ CommitteeId
                               .Include(a => a.Member)    // جلب بيانات العضو المرتبطة بـ JobId
                               .Where(a => chairmanCommitteeIds.Contains(a.CommitteeId)) // الفلترة لحصر الأنشطة بلجان الليدر فقط 🌟
                               .OrderByDescending(a => a.Date) // ترتيب من الأحدث للأقدم
                               .Take(5) // أخذ آخر 5 حركات فقط
                               .ToList(); 

            int totalMembersCount = _context.CommitteeMembers
                                    .Where(cm => chairmanCommitteeIds.Contains(cm.CommitteeId))
                                    .Select(cm => cm.JobId)
                                    .Distinct()
                                    .Count();

            ViewBag.TotalMembersCount = totalMembersCount;


            int nextMeetingsCount = _context.Calendars
                                    .Count(m => chairmanCommitteeIds.Contains(m.CommitteeId)
                                             && m.MeetingTime > DateTime.Now);
            ViewBag.NextMeetingsCount = nextMeetingsCount;

            var upcomingMeetings = _context.Calendars
                                   .Where(m => chairmanCommitteeIds.Contains(m.CommitteeId)
                                            && m.MeetingTime > DateTime.Now)
                                   .OrderBy(m => m.MeetingTime) 
                                   .ToList();
            ViewBag.UpcomingMeetings = upcomingMeetings;


            return View(recentActivities);
        }
        
   
    public IActionResult Committees()
        {
            return View();
        }
    public IActionResult CommitteeMembers()
        {
            return View();
        }
        public IActionResult Meetings()
        {
            return View();
        }

        public IActionResult CreateMeeting()
        {
            return View();
        }

        public IActionResult MeetingDetails()
        {
            return View();
        }
        public IActionResult Tasks()
        {
            return View();
        }
        public IActionResult Calendar()
        {
            return View();
        }
        public IActionResult Archive()
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
    }
 }