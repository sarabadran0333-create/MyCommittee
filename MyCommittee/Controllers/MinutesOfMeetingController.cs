using Microsoft.AspNetCore.Mvc;
using MyCommittee.Repositories;

namespace MyCommittee.Controllers
{
    public class MinutesOfMeetingController : Controller
    {
        private readonly IMinutesOfMeetingRepository _minutesRepo;

        public MinutesOfMeetingController(IMinutesOfMeetingRepository minutesRepo)
        {
            _minutesRepo = minutesRepo;
        }

        // GET: /MinutesOfMeeting
        public IActionResult Index()
        {
            var minutes = _minutesRepo.GetAll();
            return View(minutes);
        }
    }
}
