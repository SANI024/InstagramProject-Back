
public interface IFriendRequestRepository
{
    FriendRequestResponseDto SendFriendRequest(int sender_id, int reciver_id);
    FriendRequestResponseDto GetFriendRequestsByReciverId(int reciver_id);
    FriendRequestResponseDto AcceptFriendRequest(int sender_id, int reciver_id);
    FriendRequestResponseDto RejectFriendRequest(int sender_id, int reciver_id);
}