using InstagramProjectBack.Models;
using InstagramProjectBack.Repositories;
namespace InstagramProjectBack.Services
{
    public class MessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly FriendService _friendService;

        public MessageService(IMessageRepository messageRepository, FriendService friendService)
        {
            _messageRepository = messageRepository;
            _friendService = friendService;
        }

        public async Task<BaseResponseDto<Message>> ProcessMessageAsync(int senderId, int receiverId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return new BaseResponseDto<Message> { Success = false, Message = "Message cannot be empty." };
            }

            if (!_friendService.AreFriends(senderId, receiverId))
            {
                return new BaseResponseDto<Message> { Success = false, Message = "You are not friends." };
            }
            var sentMessage = await _messageRepository.SendMessageAsync(senderId, receiverId, message);
            return sentMessage;
        }

        public async Task<BaseResponseDto<List<Message>>> getMessagesAsync(int LoggedInUserId, int userId)
        {
            var result = await _messageRepository.GetMessagesAsync(LoggedInUserId, userId);
            return result;
        }

    };


}