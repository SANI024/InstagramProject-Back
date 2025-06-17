using System.Security.Claims;
using InstagramProjectBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace InstagramProjectBack.SignalR
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly MessageService _messageService;

        public ChatHub(MessageService messageService)
        {
            _messageService = messageService;
        }

        public override async Task OnConnectedAsync()
        {
          var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          Console.WriteLine($"User connected: {userId}");
          await base.OnConnectedAsync();
        }



        public async Task SendMessage(string receiverId, string message)
        {
            var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"SignalR senderId from context: {senderId}");
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