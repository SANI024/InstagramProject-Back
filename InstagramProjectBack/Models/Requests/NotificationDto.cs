namespace InstagramProjectBack.Models.Requests
{
    public class NotificationDto
    {
       
        public int RecieverId { get; set; }

        public string Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
