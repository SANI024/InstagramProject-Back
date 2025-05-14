
public enum FriendRequestStatus
{
    Pending,
    Accepted,
    Rejected
}

public class SendFriendRequestDto
{
    public int Sender_Id { get; set; }
    public int Reciver_Id { get; set; }
    public FriendRequestStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

}