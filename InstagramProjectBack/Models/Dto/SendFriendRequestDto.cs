namespace InstagramProjectBack.Models.Dto
{
    public enum FriendRequestStatus
    {
        Pending,
        Accepted,
        Rejected
    }

    public class SendFriendRequestDto
    {
        public int Reciver_Id { get; set; }
        public FriendRequestStatus Status { get; set; } = FriendRequestStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
