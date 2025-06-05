using InstagramProjectBack.Models.Interfaces;
using InstagramProjectBack.Models.Requests;
using InstagramProjectBack.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace InstagramProjectBack.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IHubContext<NotificationHub> _hub;

        public NotificationRepository(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task SendNotificationAsync( NotificationDto notification)
        {
            await _hub.Clients.User(notification.RecieverId.ToString())
                .SendAsync("ReceiveNotification", notification);
        }
    }
}
