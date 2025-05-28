using InstagramProjectBack.Models;
using InstagramProjectBack.Repositories;
using Microsoft.AspNetCore.SignalR;

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

            return _messageRepository.SendMessage(senderId, receiverId, message);
        }

    };


}