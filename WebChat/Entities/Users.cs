using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebChat.Entities
{
    [Index("Username", IsUnique = true)]
    public class Users
    {
        public Users()
        {
            ReceivedMessages = new HashSet<Messages>();
            SentMessages = new HashSet<Messages>();
        }
        private string username;
        public int Id { get; set; }
        [MaxLength(50)]
        public string Username { get => username; set => username = value.Trim().ToLower(); }
        public string Password { get; set; }
        [MaxLength(200)]
        public string FullName { get; set; }


        public ICollection<Messages> SentMessages { get; set; }
        public ICollection<Messages> ReceivedMessages { get; set; }
    }
}
