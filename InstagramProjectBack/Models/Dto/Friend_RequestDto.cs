using System.ComponentModel.DataAnnotations.Schema;

using InstagramProjectBack.Models.Dto;

namespace InstagramProjectBack.Models
{
    public class Friend_RequestDto
    {
        public int Id { get; set; }
        public int Sender_Id { get; set; }
        public UserDto Sender { get; set; }
        public int Reciver_Id { get; set; }
        public UserDto Reciver { get; set; }
        public FriendRequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
