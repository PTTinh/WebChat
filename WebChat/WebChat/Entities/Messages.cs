namespace WebChat.Entities
{
    public class Messages
    {
        public Messages() { }
        public long Id { get; set; }
        public string Message { get; set; }
        public DateTime SendingTime { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public Users Sender { get; set; }
        public Users Receiver { get; set; }
    }
}
