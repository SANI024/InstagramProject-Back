using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Requests;
using InstagramProjectBack.Models.Interfaces;

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

        public BaseResponseDto<Friend_Request> SendFriendRequestService(int sender_id, int reciver_id)
        {
            var status = _friendRequestRepo.SendFriendRequest(sender_id, reciver_id);
            if (status.Success == false)
            {
                return new BaseResponseDto<Friend_Request>
                {
                    Data = null,
                    Message = "User Doesn't Exists.",
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
                return new BaseResponseDto<Friend_Request>
                {
                    Success = status.Success,
                    Data = status.Data,
                    Message = "error sending message"

                };
            }

            return new BaseResponseDto<Friend_Request>
            {
                Success = status.Success,
                Data = status.Data,
                Message = status.Message

            };

        }

        public BaseResponseDto<List<Friend_Request>> GetFriendRequestsService(int userId)
        {
            BaseResponseDto<List<Friend_Request>> FriendRequests = _friendRequestRepo.GetFriendRequestsByReciverId(userId);
            return new BaseResponseDto<List<Friend_Request>>
            {
                Data = FriendRequests.Data,
                Message = FriendRequests.Message,
                Success = FriendRequests.Success
            };
        }

        public BaseResponseDto<Friend_Request> AcceptFriendRequestService(int sender_id, int receiver_id)
        {
            BaseResponseDto<Friend_Request> result = _friendRequestRepo.AcceptFriendRequest(sender_id, receiver_id);
            return new BaseResponseDto<Friend_Request>
            {
                Data = result.Data,
                Message = result.Message,
                Success = result.Success
            };
        }
        
         public BaseResponseDto<Friend_Request> RejectFriendRequestService(int sender_id,int receiver_id)
        {
            BaseResponseDto<Friend_Request> result = _friendRequestRepo.RejectFriendRequest(sender_id,receiver_id);
            return new BaseResponseDto<Friend_Request>
            {
                Data = result.Data,
                Message = result.Message,
                Success = result.Success
            };
        }
    }
}
