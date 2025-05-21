using InstagramProjectBack.Repositories;
using InstagramProjectBack.Services;
using Microsoft.AspNetCore.SignalR;

namespace InstagramProjectBack.Models
{
    public class ChatHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly FriendService _friendService;
        public ChatHub(IMessageRepository messageRepository, FriendService friendService)
        {
            _messageRepository = messageRepository;
            _friendService = friendService;
        }
        public async Task SendMessage(string SenderId, string ReciverId, string Message)
        {
            if (string.IsNullOrWhiteSpace(Message))
            {
                await Clients.Caller.SendAsync("MessageFailed", "Message cannot be empty.");
                return;
            }
            if (!int.TryParse(SenderId, out int senderIntId) || !int.TryParse(ReciverId, out int reciverIntId))
            {
                await Clients.Caller.SendAsync("MessageFailed", $"Problem on id parsing.");
                return;
            }
            bool AreFriends = _friendService.AreFriends(senderIntId, reciverIntId);
            if (AreFriends == false)
            {
                await Clients.Caller.SendAsync("MessageFailed", "You are not friends.");
                return;
            }

            BaseResponseDto<Message> SentMessage = _messageRepository.SendMessage(senderIntId, reciverIntId, Message);
            if (SentMessage.Success == false)
            {
                await Clients.Caller.SendAsync("MessageFailed", $"{SentMessage.Message}");
                return;
            }
            await Clients.User(ReciverId).SendAsync("ReceiveMessage", SenderId, Message);
            await Clients.User(SenderId).SendAsync("MessageSent", Message);
        }
    }
}