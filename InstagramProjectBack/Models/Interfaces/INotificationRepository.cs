using InstagramProjectBack.Models.Requests;

namespace InstagramProjectBack.Models.Interfaces
{
    public interface INotificationRepository
    {
        Task SendNotificationAsync( NotificationDto notification);
    }
}
