using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCommittee.Data;
using MyCommittee.Models;
using MyCommittee.ViewModels;

namespace MyCommittee.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Members.FirstOrDefault(m =>
                    m.Username == model.Username &&
                    m.Password == model.Password);

                if (user != null)
                {
                    int userRole = user.RoleId;

                    if (userRole == 1)
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }

                    else if (userRole == 2)
                    {
                        return RedirectToAction("Dashboard", "Chairman");
                    }

                    else if (userRole == 3)
                    {
                        return RedirectToAction("Dashboard", "Member");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Username or Password are incorrect");
                }
            }

            return View(model);
        }
    }
}