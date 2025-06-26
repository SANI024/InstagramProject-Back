using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Requests;
using InstagramProjectBack.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstagramProjectBack.Services
{
    public class FriendRequestService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IFriendRequestRepository _friendRequestRepo;

        public FriendRequestService(INotificationRepository notificationrepo, IFriendRequestRepository friendRequestRepo)
        {
            _notificationRepository = notificationrepo;
            _friendRequestRepo = friendRequestRepo;
        }

        public async Task<BaseResponseDto<Friend_RequestDto>> SendFriendRequestServiceAsync(int sender_id, int reciver_id)
        {
            var status = await _friendRequestRepo.SendFriendRequestAsync(sender_id, reciver_id);
            if (!status.Success)
            {
                return new BaseResponseDto<Friend_RequestDto>
                {
                    Data = null,
                    Message = status.Message,
                    Success = false,
                };
            }

            var notification = new NotificationDto
            {
                RecieverId = reciver_id,
                Type = "friend request",
                CreatedAt = DateTime.Now,
                IsRead = false,
            };

            var sendNotif = _notificationRepository.SendNotificationAsync(notification);

            if (sendNotif == null)
            {
                return new BaseResponseDto<Friend_RequestDto>
                {
                    Success = status.Success,
                    Data = status.Data,
                    Message = "Error sending notification"
                };
            }

            return new BaseResponseDto<Friend_RequestDto>
            {
                Success = status.Success,
                Data = status.Data,
                Message = status.Message
            };
        }

        public async Task<BaseResponseDto<List<Friend_RequestDto>>> GetFriendRequestsServiceAsync(int userId)
        {
            var friendRequests = await _friendRequestRepo.GetFriendRequestsByReciverIdAsync(userId);
            return new BaseResponseDto<List<Friend_RequestDto>>
            {
                Data = friendRequests.Data,
                Message = friendRequests.Message,
                Success = friendRequests.Success
            };
        }

        public async Task<BaseResponseDto<Friend_RequestDto>> AcceptFriendRequestServiceAsync(int sender_id, int receiver_id)
        {
            var result = await _friendRequestRepo.AcceptFriendRequestAsync(sender_id, receiver_id);
            return new BaseResponseDto<Friend_RequestDto>
            {
                Data = result.Data,
                Message = result.Message,
                Success = result.Success
            };
        }

        public async Task<BaseResponseDto<Friend_RequestDto>> RejectFriendRequestServiceAsync(int sender_id, int receiver_id)
        {
            var result = await _friendRequestRepo.RejectFriendRequestAsync(sender_id, receiver_id);
            return new BaseResponseDto<Friend_RequestDto>
            {
                Data = result.Data,
                Message = result.Message,
                Success = result.Success
            };
        }

        public async Task<BaseResponseDto<List<UserDto>>> getFriendsServiceAsync(int userId)
        {
            var result = await _friendRequestRepo.getFriendsAsync(userId);
            return new BaseResponseDto<List<UserDto>>
            {
                Data = result.Data,
                Message = result.Message,
                Success = result.Success
            }
            ;
        }

        public async Task<BaseResponseDto<Friend_RequestDto>> FriendRequestStatusServiceAsync(int userId, int checkerId)
        {
            var result = await _friendRequestRepo.GetFriendRequestStatusAsync(userId, checkerId);
            return new BaseResponseDto<Friend_RequestDto>
            {
                Data = result.Data,
                Message = result.Message,
                Success = result.Success
            };
        }
    }
}
