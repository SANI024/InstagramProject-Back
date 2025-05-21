using InstagramProjectBack.Data;
using InstagramProjectBack.Models;

namespace InstagramProjectBack.Repositories
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly AppDbContext _context;

        public FriendRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public BaseResponseDto<Friend_Request> SendFriendRequest(int sender_id, int reciver_id)
        {
            var FriendRequestExists = _context.Friend_Requests
                .FirstOrDefault(fr => fr.Sender_Id == sender_id && fr.Reciver_Id == reciver_id);

            if (FriendRequestExists != null)
            {
                return new BaseResponseDto<Friend_Request>
                {
                    Success = false,
                    Message = "Friend request already sent.",
                    Data = null
                };
            }

            var NewFriendRequest = new Friend_Request
            {
                Sender_Id = sender_id,
                Reciver_Id = reciver_id,
                Status = FriendRequestStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Friend_Requests.Add(NewFriendRequest);
            _context.SaveChanges();

            return new BaseResponseDto<Friend_Request>
            {
                Success = true,
                Message = "Friend request sent successfully.",
                Data = NewFriendRequest
            };
        }

        public BaseResponseDto<List<Friend_Request>> GetFriendRequestsByReciverId(int reciver_id)
        {
            var FriendRequests = _context.Friend_Requests
                .Where(fr => fr.Reciver_Id == reciver_id && fr.Status == FriendRequestStatus.Pending)
                .ToList();

            if (FriendRequests.Count == 0)
            {
                return new BaseResponseDto<List<Friend_Request>>
                {
                    Success = false,
                    Message = "No requests.",
                    Data = null
                };
            }

            return new BaseResponseDto<List<Friend_Request>>
            {
                Success = true,
                Message = "Successfully fetched friend requests.",
                Data = FriendRequests
            };
        }

        public BaseResponseDto<Friend_Request> AcceptFriendRequest(int sender_id, int reciver_id)
        {
            var FriendRequest = _context.Friend_Requests
                .FirstOrDefault(fr => fr.Sender_Id == sender_id && fr.Reciver_Id == reciver_id);

            if (FriendRequest == null)
            {
                return new BaseResponseDto<Friend_Request>
                {
                    Success = false,
                    Message = "Friend request doesn't exist.",
                    Data = null
                };
            }

            if (FriendRequest.Status != FriendRequestStatus.Pending)
            {
                return new BaseResponseDto<Friend_Request>
                {
                    Success = false,
                    Message = "Friend request is not in a pending state.",
                    Data = null
                };
            }

            FriendRequest.Status = FriendRequestStatus.Accepted;
            _context.SaveChanges();

            return new BaseResponseDto<Friend_Request>
            {
                Success = true,
                Message = "Accepted friend request.",
                Data = FriendRequest
            };
        }

        public BaseResponseDto<Friend_Request> RejectFriendRequest(int sender_id, int reciver_id)
        {
            var FriendRequest = _context.Friend_Requests
                .FirstOrDefault(fr => fr.Sender_Id == sender_id && fr.Reciver_Id == reciver_id);

            if (FriendRequest == null)
            {
                return new BaseResponseDto<Friend_Request>
                {
                    Success = false,
                    Message = "Friend request doesn't exist.",
                    Data = null
                };
            }

            if (FriendRequest.Status != FriendRequestStatus.Pending)
            {
                return new BaseResponseDto<Friend_Request>
                {
                    Success = false,
                    Message = "Friend request is not in a pending state.",
                    Data = null
                };
            }

            FriendRequest.Status = FriendRequestStatus.Rejected;
            _context.SaveChanges();

            return new BaseResponseDto<Friend_Request>
            {
                Success = true,
                Message = "Rejected friend request.",
                Data = FriendRequest
            };
        }
    }
}
