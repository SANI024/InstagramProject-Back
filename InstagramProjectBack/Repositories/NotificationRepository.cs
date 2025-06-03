using InstagramProjectBack.Services;
using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
using Microsoft.AspNetCore.Identity;
using InstagramProjectBack.Data;
using InstagramProjectBack.Models.Interfaces;
using InstagramProjectBack.Models.Requests;
using Microsoft.EntityFrameworkCore;
namespace InstagramProjectBack.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;
        public NotificationRepository( AppDbContext context)
        {
            _context = context;
        }
        public BaseResponseDto<List<NotificationDto>> GetUserNotifications(int receiverId)
        {
            var notifications = _context.Notifications
        .Where(n => n.ReceiverId == receiverId)
        .OrderByDescending(n => n.CreatedAt)
        .ToList();

            if (notifications.Count == 0)
            {
                return new BaseResponseDto<List<NotificationDto>>
                {
                    Data = new List<NotificationDto>(),
                    Message = "No notifications found.",
                    Success = true
                };
            }

            var result = new List<NotificationDto>();

            foreach (var n in notifications)
            {
                string message = "";

                switch (n.Type)
                {
                    case "FriendRequest":
                        var sender = _context.Users.Find(n.RelatedEntityId);
                        message = $"{sender?.Name ?? "Someone"} sent you a friend request.";
                        break;

                    case "FriendAccepted":
                        var friend = _context.Users.Find(n.RelatedEntityId);
                        message = $"{friend?.Name ?? "Someone"} accepted your friend request.";
                        break;

                    case "Like":
                        var liker = _context.Users.Find(n.RelatedEntityId);
                        message = $"{liker?.Name ?? "Someone"} liked your post.";
                        break;

                    case "Comment":
                        var commenter = _context.Users.Find(n.RelatedEntityId);
                        message = $"{commenter?.Name ?? "Someone"} commented on your post.";
                        break;

                    default:
                        message = "You have a new notification.";
                        break;
                }

                result.Add(new NotificationDto
                {
                    Id = n.Id,
                    Type = n.Type,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt,
                });
            }

            return new BaseResponseDto<List<NotificationDto>>
            {
                Data = result,
                Success = true,
                Message = "Notifications fetched successfully."
            };
        }
    }
}
