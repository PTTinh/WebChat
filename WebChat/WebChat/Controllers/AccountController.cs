using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebChat.Entities;
using WebChat.ViewModels.Account;

namespace WebChat.Controllers
{
    public class AccountController : Controller
    {
        private readonly WebChatContext _db;
        public AccountController(WebChatContext db)
        {
            _db = db;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVM m)
        {
            if (!ModelState.IsValid)
            {
                return View(m);
            }
            var user = _db.Users.FirstOrDefault(x => x.Username == m.Username);
            if (user == null)
            {
                ModelState.AddModelError("Username", "Username không tồn tại");
                return View(m);
            }
            if (!BCrypt.Net.BCrypt.Verify(m.Password, user.Password))
            {
                ModelState.AddModelError("Password", "Password không đúng");
                return View(m);
            }
            //login user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("FullName", user.FullName)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = m.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) //lưu cookies trong 30 phút
            };
            HttpContext.SignInAsync("Cookies", claimsPrincipal, authProperties).Wait();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterVM m)
        {
            if (!ModelState.IsValid)
            {
                return View(m);
            }
            if (_db.Users.Any(x => x.Username == m.Username))
            {

                ModelState.AddModelError("Username", "Username đã tồn tại");
                return View(m);
            }
            var user = new Users();
            user.FullName = m.FullName;
            user.Username = m.Username;
            user.Password = BCrypt.Net.BCrypt.HashPassword(m.Password);
            _db.Users.Add(user);
            _db.SaveChanges();
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login");
        }

    }
}
