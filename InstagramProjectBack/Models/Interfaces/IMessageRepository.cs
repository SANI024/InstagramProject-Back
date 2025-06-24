using InstagramProjectBack.Models;

namespace InstagramProjectBack.Repositories
{
    public interface IMessageRepository
    {
        Task<BaseResponseDto<List<Message>>> GetMessagesAsync(int userId, int reciverId);
        Task<BaseResponseDto<Message>> SendMessageAsync(int senderId, int reciverId, string message);

    }
}