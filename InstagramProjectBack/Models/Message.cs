namespace InstagramProjectBack.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int SenderId { get; set; }
        public int ReciverId { get; set; }
    }
}
