using InstagramProjectBack.Services;
using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
using Microsoft.AspNetCore.Identity;
using InstagramProjectBack.Data;
using InstagramProjectBack.Models.Interfaces;
using InstagramProjectBack.Models.Requests;
using Microsoft.EntityFrameworkCore;
using InstagramProjectBack.Data;
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
