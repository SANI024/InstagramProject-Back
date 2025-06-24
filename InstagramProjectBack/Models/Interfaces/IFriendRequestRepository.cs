using InstagramProjectBack.Models;

public interface IFriendRequestRepository
{
    Task<BaseResponseDto<Friend_Request>> SendFriendRequestAsync(int sender_id, int reciver_id);
    Task<BaseResponseDto<List<Friend_Request>>> GetFriendRequestsByReciverIdAsync(int reciver_id);
    Task<BaseResponseDto<Friend_Request>> AcceptFriendRequestAsync(int sender_id, int reciver_id);
    Task<BaseResponseDto<Friend_Request>> RejectFriendRequestAsync(int sender_id, int reciver_id);
    Task<BaseResponseDto<List<User>>> getFriendsAsync(int userId);
    Task<BaseResponseDto<Friend_Request>> isFriendAsync(int userId, int checkerId);
}
