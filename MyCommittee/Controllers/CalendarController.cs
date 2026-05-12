using Microsoft.AspNetCore.Mvc;
using MyCommittee.Repositories;

namespace MyCommittee.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ICalendarRepository _calendarRepo;

        public CalendarController(ICalendarRepository calendarRepo)
        {
            _calendarRepo = calendarRepo;
        }

        public IActionResult Index()
        {
            var meetings = _calendarRepo.GetAll();
            return View(meetings);
        }
    }
}
