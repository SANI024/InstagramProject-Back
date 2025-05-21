using InstagramProjectBack.Repositories;
using InstagramProjectBack.Services;
using Microsoft.AspNetCore.SignalR;

namespace InstagramProjectBack.Models
{
    public class ChatHub : Hub
    {
        private readonly MessageService _messageService;

        public ChatHub(MessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task SendMessage(string senderId, string receiverId, string message)
        {
            if (!int.TryParse(senderId, out int senderIntId) || !int.TryParse(receiverId, out int receiverIntId))
            {
                await Clients.Caller.SendAsync("MessageFailed", "Invalid IDs.");
                return;
            }

            var result = await _messageService.ProcessMessageAsync(senderIntId, receiverIntId, message);
            if (!result.Success)
            {
                await Clients.Caller.SendAsync("MessageFailed", result.Message);
                return;
            }

            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);
            await Clients.User(senderId).SendAsync("MessageSent", message);
        }
    }

}