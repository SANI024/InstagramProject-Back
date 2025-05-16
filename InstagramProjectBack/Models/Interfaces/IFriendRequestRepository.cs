
using InstagramProjectBack.Models;

public interface IFriendRequestRepository
{
    BaseResponseDto<Friend_Request> SendFriendRequest(int sender_id, int reciver_id);
    BaseResponseDto<List<Friend_Request>> GetFriendRequestsByReciverId(int reciver_id);
    BaseResponseDto<Friend_Request> AcceptFriendRequest(int sender_id, int reciver_id);
    BaseResponseDto<Friend_Request> RejectFriendRequest(int sender_id, int reciver_id);
}