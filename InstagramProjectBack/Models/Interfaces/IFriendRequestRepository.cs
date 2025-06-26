using InstagramProjectBack.Models;

public interface IFriendRequestRepository
{
    Task<BaseResponseDto<Friend_RequestDto>> SendFriendRequestAsync(int sender_id, int reciver_id);
    Task<BaseResponseDto<List<Friend_RequestDto>>> GetFriendRequestsByReciverIdAsync(int reciver_id);
    Task<BaseResponseDto<Friend_RequestDto>> AcceptFriendRequestAsync(int sender_id, int reciver_id);
    Task<BaseResponseDto<Friend_RequestDto>> RejectFriendRequestAsync(int sender_id, int reciver_id);
    Task<BaseResponseDto<List<UserDto>>> getFriendsAsync(int userId);
    Task<BaseResponseDto<Friend_RequestDto>> GetFriendRequestStatusAsync(int userId, int checkerId);
}
