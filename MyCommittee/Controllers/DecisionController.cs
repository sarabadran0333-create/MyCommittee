using Microsoft.AspNetCore.Mvc;
using MyCommittee.Repositories;

namespace MyCommittee.Controllers
{
    public class DecisionController : Controller
    {
        private readonly IDecisionRepository _decisionRepo;

        public DecisionController(IDecisionRepository decisionRepo)
        {
            _decisionRepo = decisionRepo;
        }

        // GET: /Decision
        public IActionResult Index()
        {
            var decisions = _decisionRepo.GetAll();
            return View(decisions);
        }
    }
}
