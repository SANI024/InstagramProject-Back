namespace InstagramProjectBack.Models.Requests
{
    public class NotificationDto
    {
       
        public int Id { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
