using Microsoft.AspNetCore.Mvc;
using MyCommittee.Repositories;

namespace MyCommittee.Controllers
{
    public class CommitteeController : Controller
    {
        private readonly ICommitteeRepository _committeeRepo;

        public CommitteeController(ICommitteeRepository committeeRepo)
        {
            _committeeRepo = committeeRepo;
        }

        // GET: /Committee
        public IActionResult Index()
        {
            var committees = _committeeRepo.GetAll();
            return View(committees);
        }
    }
}
