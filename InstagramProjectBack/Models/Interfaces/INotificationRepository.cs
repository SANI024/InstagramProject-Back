using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Requests;

namespace InstagramProjectBack.Models.Interfaces
{
    public interface INotificationRepository
    {
        BaseResponseDto<List<NotificationDto>> GetUserNotifications(int userId);

    }
}
