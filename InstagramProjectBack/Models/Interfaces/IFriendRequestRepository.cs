using InstagramProjectBack.Models;

public interface IFriendRequestRepository
{
    object SendFriendRequest(int sender_id, int reciver_id);
    List<Friend_Request> GetFriendRequestsByReciverId(int reciver_id);
    object AcceptFriendRequest(int sender_id, int reciver_id);
    object RejectFriendRequest(int sender_id, int reciver_id);
}