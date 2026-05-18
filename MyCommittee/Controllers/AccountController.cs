
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MyCommittee.Data;
using MyCommittee.ViewModels;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;





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

        // 1. لعرض الصفحة عند الضغط على الرابط
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // 2. لمعالجة البيانات عند الضغط على الزر
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _context.Members.FirstOrDefaultAsync(m => m.EMail == email);

            if (user != null)
            {
                // رابط تجريبي يوجه لصفحة ResetPassword التي سنصنعها
                string resetLink = Url.Action("ResetPassword", "Account", new { email = email }, Request.Scheme);

                string message = $"<h3>Password recovery request</h3>" +
                                 $"<p>To reset your password, click the link below:</p>" +
                                 $"<a href='{resetLink}'>Click here to reset</a>" +
                                 $"<p>If you did not request this, please ignore this email.</p>";

                await SendEmailAsync(email, "Reset Your Password", message);

                TempData["SuccessMessage"] = "The appointment link has been successfully sent to your email.";
                return View();
            }

            ModelState.AddModelError("", "Email address not registered.");
            return View();
        }

        public async Task SendEmailAsync(string userEmail, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("ayakh4946@gmail.com")); // إيميلك الشخصي
            email.To.Add(MailboxAddress.Parse(userEmail));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

            // حددي المكتبة مباشرة عند إنشاء الكائن
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            // إعدادات Gmail SMTP
            // جربي هذا التعديل
            await smtp.ConnectAsync("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
            await smtp.AuthenticateAsync("ayakhallof@gmail.com", "qqmiukoobkijnmrv"); // كلمة مرور التطبيق وليس الباسورد العادي
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login");

            // نمرر الإيميل للموديل لنعرف أي مستخدم سنقوم بتغيير باسورد له
            var model = new ResetPasswordViewModel { Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            // 1. التحقق اليدوي من تطابق الكلمتين
            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "The passwords do not match..");
            }

            if (ModelState.IsValid)
            {
                // 2. البحث عن المستخدم (تأكدي من اسم العمود EMail أو Email كما في قاعدة بياناتك)
                var user = await _context.Members.FirstOrDefaultAsync(m => m.EMail == model.Email);

                if (user != null)
                {
                    // 3. تحديث كلمة السر
                    user.Password = model.NewPassword;

                    // 4. إخبار الداتابيس أن هذا الحقل تم تعديله
                    _context.Entry(user).State = EntityState.Modified;

                    // 5. حفظ التغييرات
                    await _context.SaveChangesAsync();

                    // 6. التوجيه لصفحة الدخول مع رسالة نجاح
                    TempData["SuccessMsg"] = "Password updated successfully!";
                    // حددي اسم الأكشن واسم الكنترولر بوضوح لضمان الانتقال الصحيح
                    return RedirectToAction("Index", "Home", null);
                }
                else
                {
                    ModelState.AddModelError("", "User not found.");
                }
            }

            // إذا فشل أي شيء، نرجع لنفس الصفحة مع عرض الأخطاء
            return View(model);
        }
    }

}
