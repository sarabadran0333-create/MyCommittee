using Microsoft.AspNetCore.Mvc;
using MyCommittee.Models;
using MyCommittee.Repositories; 

namespace MyCommittee.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberRepository _memberRepo;

        public MemberController(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo;
        }

        // GET: Member
        public IActionResult Index()
        {
            var members = _memberRepo.GetAll();
            return View(members);
        }

        // GET: Member/Details/5
        public IActionResult Details(int id)
        {
            var member = _memberRepo.GetById(id);
            if (member == null)
                return NotFound();

            return View(member);
        }

        // GET: Member/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Member/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Member member)
        {
            if (!ModelState.IsValid)
                return View(member);



            _memberRepo.Add(member);
            return RedirectToAction(nameof(Index));
        }

        // GET: Member/Edit/5
        public IActionResult Edit(int id)
        {
            var member = _memberRepo.GetById(id);
            if (member == null)
                return NotFound();

            return View(member);
        }

        // POST: Member/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Member member)
        {
            if (id != member.JobId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(member);

            var existing = _memberRepo.GetById(id);
            if (existing == null)
                return NotFound();

            _memberRepo.Update(member);
            return RedirectToAction(nameof(Index));
        }

        // GET: Member/Delete/5
        public IActionResult Delete(int id)
        {
            var member = _memberRepo.GetById(id);
            if (member == null)
                return NotFound();

            return View(member);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var existing = _memberRepo.GetById(id);
            if (existing == null)
                return NotFound();

            _memberRepo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
