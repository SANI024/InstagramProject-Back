using System.Security.Claims;
using InstagramProjectBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace InstagramProjectBack.Models
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly MessageService _messageService;

        public ChatHub(MessageService messageService)
        {
            _messageService = messageService;
        }


        public async Task SendMessage(string receiverId, string message)
        {
            var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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