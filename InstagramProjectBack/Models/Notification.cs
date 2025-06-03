namespace InstagramProjectBack.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int ReceiverId { get; set; }
        public string Type { get; set; } 
        public int? RelatedEntityId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }

        public User Receiver { get; set; }
    }
}
