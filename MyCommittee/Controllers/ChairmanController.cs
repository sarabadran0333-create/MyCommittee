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
            var chairmanCommitteeIds = _context.CommitteeMembers
                                       .Where(c => c.JobId == userId)
                                       .Select(c => c.CommitteeId)
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
            return View();
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