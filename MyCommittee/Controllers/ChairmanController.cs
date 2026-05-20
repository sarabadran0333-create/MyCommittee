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
                               .Where(a => chairmanCommitteeIds.Contains(a.CommitteeId)) // الفلترة لحصر الأنشطة بلجان الليدر فقط 
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
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var committees = _context.CommitteeMembers
                .Where(cm => cm.JobId == userId)
                .Join(_context.Committees,
                      cm => cm.CommitteeId,
                      c => c.CommitteeId,
                      (cm, c) => new
                      {
                          c.CommitteeId,
                          c.Title,
                          c.Start,
                          c.End,
                          c.TypeId,

                          MembersCount = _context.CommitteeMembers
                              .Count(x => x.CommitteeId == c.CommitteeId),

                          Members = _context.CommitteeMembers
                              .Where(x => x.CommitteeId == c.CommitteeId)
                              .Join(_context.Members,
                                    x => x.JobId,
                                    m => m.JobId,
                                    (x, m) => m.Username)
                              .ToList(),

                          Permissions = _context.Permissions
                        .Where(p => p.CommitteeId == c.CommitteeId)
                         .Select(p => new
                          {
                          p.Title,
                          p.Description
                           })
                     .ToList()
                      })
                .ToList();

            return View(committees);
        }
        public IActionResult CommitteeMembers()
        {
            return View();
        }
        public IActionResult Meetings()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userCommitteeIds = _context.CommitteeMembers
                                   .Where(cm => cm.JobId == userId)
                                   .Select(cm => cm.CommitteeId)
                                   .ToList();

            var meetingsList = _context.Calendars
                               .Include(m => m.Committee)
                               .Where(m => userCommitteeIds.Contains(m.CommitteeId))
                               .OrderByDescending(m => m.MeetingTime)
                               .ToList();

            return View(meetingsList);
        }

        

        [HttpGet]
        public IActionResult CreateMeeting()
        {
            int? currentLeaderId = HttpContext.Session.GetInt32("UserId");
            if (currentLeaderId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var myCommittees = _context.CommitteeMembers
                                       .Where(c => c.JobId == currentLeaderId)
                                       .Select(c => c.CommitteeId)
                                       .ToList();

            var chairmanCommittees = _context.Committees
                                             .Where(c => myCommittees.Contains(c.CommitteeId))
                                             .ToList();
            ViewBag.Committees = chairmanCommittees;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateMeeting(string MeetingTitle, int CommitteeId, DateTime? Date, TimeSpan? Time, string Place)
        {
            int? currentLeaderId = HttpContext.Session.GetInt32("UserId");
            if (currentLeaderId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            //  التحقق من ملء الحقول الأساسية
            if (string.IsNullOrEmpty(MeetingTitle) || Date == null || Time == null || CommitteeId == 0)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields (Title, Committee, Date, and Time).";
                RechargeCommittees(currentLeaderId.Value); 
                return View();
            }

            try
            {
              
                DateTime fullMeetingTime = Date.Value.Date + Time.Value;


                // ================= كود منع تعارض المواعيد للأعضاء =================

               
                var currentCommitteeMembers = _context.CommitteeMembers
                                                      .Where(m => m.CommitteeId == CommitteeId)
                                                      .Select(m => m.JobId)
                                                      .ToList();

                // البحث عن أي اجتماع آخر مجدول في نفس الوقت واليوم تماماً للأعضاء
                var conflictingMeetingExists = _context.Calendars
                    .Where(c => c.MeetingTime == fullMeetingTime)
                    .Any(c => _context.CommitteeMembers
                        .Where(m => m.CommitteeId == c.CommitteeId)
                        .Any(m => currentCommitteeMembers.Contains(m.JobId))
                    );

                //  في حال وجود تعارض: نترك المستخدم في صفحة الإنشاء ونظهر الرسالة الحمراء هناك
                if (conflictingMeetingExists)
                {
                    TempData["ErrorMessage"] = "Cannot create meeting! One or more committee members have another conflicting meeting at the exact same time.";
                    RechargeCommittees(currentLeaderId.Value); 
                    return View(); 
                }

                // =======================================================================

                //  إذا لم يكن هناك تعارض، يتم إنشاء الاجتماع وحفظه بنجاح
                var newMeeting = new Calendar
                {
                    MeetingTitle = MeetingTitle,
                    CommitteeId = CommitteeId,
                    MeetingTime = fullMeetingTime,
                    Place = string.IsNullOrEmpty(Place) ? "N/A" : Place
                };

                _context.Calendars.Add(newMeeting);
                _context.SaveChanges();

               
                TempData["SuccessMessage"] = "Meeting created successfully with no conflicts!";
                return RedirectToAction(nameof(Meetings)); 
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to create the meeting. Error: " + ex.Message;
                RechargeCommittees(currentLeaderId.Value);
                return View();
            }
        }

        
        private void RechargeCommittees(int leaderId)
        {
            var myCommittees = _context.CommitteeMembers
                                       .Where(c => c.JobId == leaderId)
                                       .Select(c => c.CommitteeId)
                                       .ToList();

            ViewBag.Committees = _context.Committees
                                         .Where(c => myCommittees.Contains(c.CommitteeId))
                                         .ToList();
        }
        

        public IActionResult Minutes()
        {
            return View();
        }
        public IActionResult CreateMinutes()
        {
            return View();
        }
        public IActionResult Tasks()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var leaderCommitteeIds = _context.CommitteeMembers
                .Where(cm => cm.JobId == userId)
                .Select(cm => cm.CommitteeId)
                .ToList();

            var tasks = _context.Tasks
                .Include(t => t.Member)
                .Include(t => t.Committee)
                .Where(t => leaderCommitteeIds.Contains(t.CommitteeId))
                .ToList();
            ViewBag.TotalTasks = tasks.Count();

            ViewBag.CompletedTasks = tasks.Count(t => t.IsSubmitted);

            ViewBag.InProgressTasks = tasks.Count(t =>
                !t.IsSubmitted && t.Deadline >= DateTime.Now);
            return View(tasks);
        }
        public IActionResult Calendar()
        {
            return View();
        }
        public IActionResult Archive()
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

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMeeting(int id)
        {
           
            var meeting = _context.Calendars.Find(id);

            if (meeting != null)
            {
                _context.Calendars.Remove(meeting);
                _context.SaveChanges(); // الحذف الفعلي من الـ Database
            }

            
            return RedirectToAction(nameof(Meetings));
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMeeting(int id, string Title, DateTime MeetingTime, string Room)
        {
           
            var meeting = _context.Calendars.Find(id);
            if (meeting == null)
            {
                return NotFound();
            }

            
            meeting.MeetingTitle = Title;
            meeting.MeetingTime = MeetingTime;
            meeting.Place = Room;

            _context.SaveChanges(); // حفظ التعديلات في الداتابيس

            return RedirectToAction(nameof(Meetings)); 
        }
    }
}
 