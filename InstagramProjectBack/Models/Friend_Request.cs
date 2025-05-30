using InstagramProjectBack.Models.Dto;

namespace InstagramProjectBack.Models
{
    public class Friend_Request
    {
        public int Id { get; set; }
        public int Sender_Id { get; set; }
        public int Reciver_Id { get; set; }
        public FriendRequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
