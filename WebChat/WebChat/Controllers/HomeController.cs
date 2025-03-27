using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebChat.Models;
using WebChat.Entities;
using WebChat.Common;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.InteropServices;
using System.Collections.Immutable;
namespace WebChat.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly WebChatContext _db;

    public HomeController(ILogger<HomeController> logger, WebChatContext _db)
    {
        _logger = logger;
        this._db = _db;
    }

    //[AllowAnonymous] không cần phải đăng nhập 
    public IActionResult Index()
    {
        var currentUserId = User.GetUserId();
        var users = _db.Users.Where(x => x.Id != currentUserId).ToList();
        return View(users);
    }

    [HttpGet]
    [Route("Home/Chat/{id}/{LastMsgId?}")]
    public IActionResult Chat(int id, long? LastMsgId = null)
    {
        var currentUserId = User.GetUserId();
        var messages = _db.Messages
            .Where(x => ((x.SenderId == currentUserId && x.ReceiverId == id)
                        || (x.SenderId == id && x.ReceiverId == currentUserId))
                        && (LastMsgId == null || x.Id < LastMsgId))
            .OrderByDescending(x => x.Id)
            .Take(10)
            .ToList();
        messages.Reverse(); // sắp xếp lại theo thứ tự tăng dần
        return Json(messages);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
