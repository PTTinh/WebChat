using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebChat.Common;
using WebChat.Entities;

namespace WebChat.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly WebChatContext _db;
        public ChatHub(WebChatContext db)
        {
            this._db = db;
        }
        public async Task SendMessage(string message, int receiverId)
        {
            var senderId = Context.User.GetUserId();
            var Chat = new Messages
            {
                Message = message,
                SenderId = senderId,
                ReceiverId = receiverId,
                SendingTime = DateTime.Now
            };
            _db.Messages.Add(Chat);
            await _db.SaveChangesAsync();
            await Clients.Users(senderId.ToString(), receiverId.ToString())
                .SendAsync("ReceiveMessage", Chat);
        }
    }
}
