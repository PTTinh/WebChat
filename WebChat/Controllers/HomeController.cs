using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using WebChat.Common;
using WebChat.Entities;
using WebChat.Models;

namespace WebChat.Controllers
{
    [Authorize] // cần phải đăng nhập mới được truy cập
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebChatContext _db;
        public HomeController(ILogger<HomeController> logger, WebChatContext db)
        {
            _logger = logger;
            this._db = db;
        }

        //[AllowAnonymous] không cần phải đăng nhập 
        public IActionResult Index()
        {
            var currentUserId = User.GetUserId();
            var users = _db.Users.Where(x => x.Id != currentUserId).ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult Chat(int id)
        {
            var currentUserId = User.GetUserId();
            var messages = _db.Messages
                .Where(x => (x.SenderId == currentUserId && x.ReceiverId == id) || (x.SenderId == id && x.ReceiverId == currentUserId))
                .OrderBy(x => x.SendingTime)
                .ToList();
            return Json(messages);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
