using InstagramProjectBack.Models;

namespace InstagramProjectBack.Repositories
{
    public interface IMessageRepository
    {
        BaseResponseDto<Message> SendMessage(int SenderId, int ReciverId, string Message);
        BaseResponseDto<List<Message>> GetMessages(int UserId);
    }
}