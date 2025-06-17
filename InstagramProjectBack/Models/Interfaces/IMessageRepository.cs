using InstagramProjectBack.Models;

namespace InstagramProjectBack.Repositories
{
    public interface IMessageRepository
    {
       Task<BaseResponseDto<List<Message>>> GetMessagesAsync(int userId);
       Task<BaseResponseDto<Message>> SendMessageAsync(int senderId, int reciverId, string message);

    }
}