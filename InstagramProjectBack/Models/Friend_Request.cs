using System.ComponentModel.DataAnnotations.Schema;

using InstagramProjectBack.Models.Dto;

namespace InstagramProjectBack.Models
{
    public class Friend_Request
    {
        public int Id { get; set; }
        public int Sender_Id { get; set; }

        [ForeignKey("Sender_Id")]
        public User Sender { get; set; }
        public int Reciver_Id { get; set; }
        [ForeignKey("Reciver_Id")]
        public User Reciver { get; set; }
        public FriendRequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
