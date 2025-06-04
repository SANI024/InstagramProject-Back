using InstagramProjectBack.SignalR;
using InstagramProjectBack.Repositories;
using Microsoft.AspNetCore.SignalR;
using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Requests;

namespace InstagramProjectBack.Services
{
    public class FriendRequestService
    {
        private readonly NotificationRepository _notificationRepository;
        private readonly FriendRequestRepository _friendRequestRepo;

        public FriendRequestService(NotificationRepository notificationrepo, FriendRequestRepository friendRequestRepo)
        {
            _notificationRepository = notificationrepo;
            _friendRequestRepo = friendRequestRepo;
        }

        public BaseResponseDto<Friend_Request> SendFriendRequestService(int sender_id, int reciver_id)
        {
            var status = _friendRequestRepo.SendFriendRequest(sender_id, reciver_id);
            if (status.Success == false)
            {
                return  new BaseResponseDto<Friend_Request>
                {
                    Data = null,
                    Message = "User Doesn't Exists.",
                    Success = false,
                };
            }
          var notification =  new NotificationDto   {
                
                RecieverId = reciver_id,
                Type="friend request",
                CreatedAt = DateTime.Now,
                IsRead = false,
            };


            var sendNotif = _notificationRepository.SendNotificationAsync(notification);

           if(sendNotif== null)
            {
                return   new BaseResponseDto<Friend_Request>
                {
                    Success = status.Success,
                    Data = status.Data,
                    Message = "error sending message"

                };
            }

            return new BaseResponseDto<Friend_Request> {
                Success= status.Success,
                Data = status.Data,
                Message = status.Message

            };




        }




    }
}
