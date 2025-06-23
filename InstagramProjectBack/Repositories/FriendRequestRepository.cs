using InstagramProjectBack.Data;
using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;

using Microsoft.EntityFrameworkCore;

namespace InstagramProjectBack.Repositories
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly AppDbContext _context;

        public FriendRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponseDto<Friend_Request>> SendFriendRequestAsync(int sender_id, int reciver_id)
        {
            if (sender_id == reciver_id)
            {
                return new BaseResponseDto<Friend_Request>
                {
                    Success = false,
                    Message = "You cannot send a friend request to yourself.",
                    Data = null
                };
            }
            var friendRequestExists = await _context.Friend_Requests
                .FirstOrDefaultAsync(fr => fr.Sender_Id == sender_id && fr.Reciver_Id == reciver_id);

            if (friendRequestExists != null)
            {
                return new BaseResponseDto<Friend_Request>
                {
                    Success = false,
                    Message = "Friend request already sent.",
                    Data = null
                };
            }

            var newFriendRequest = new Friend_Request
            {
                Sender_Id = sender_id,
                Reciver_Id = reciver_id,
                Status = FriendRequestStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Friend_Requests.AddAsync(newFriendRequest);
            await _context.SaveChangesAsync();

            return new BaseResponseDto<Friend_Request>
            {
                Success = true,
                Message = "Friend request sent successfully.",
                Data = newFriendRequest
            };
        }

        public async Task<BaseResponseDto<List<Friend_Request>>> GetFriendRequestsByReciverIdAsync(int reciver_id)
        {
            var friendRequests = await _context.Friend_Requests
                .Include(fr => fr.Sender)
                .Where(fr => fr.Reciver_Id == reciver_id && fr.Status == FriendRequestStatus.Pending)
                .ToListAsync();

            if (friendRequests.Count == 0)
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
                Data = friendRequests
            };
        }

        public async Task<BaseResponseDto<Friend_Request>> AcceptFriendRequestAsync(int sender_id, int reciver_id)
        {
            var friendRequest = await _context.Friend_Requests
                .FirstOrDefaultAsync(fr => fr.Sender_Id == sender_id && fr.Reciver_Id == reciver_id);

            if (friendRequest == null)
            {
                return new BaseResponseDto<Friend_Request>
                {
                    Success = false,
                    Message = "Friend request doesn't exist.",
                    Data = null
                };
            }

            if (friendRequest.Status != FriendRequestStatus.Pending)
            {
                return new BaseResponseDto<Friend_Request>
                {
                    Success = false,
                    Message = "Friend request is not in a pending state.",
                    Data = null
                };
            }

            friendRequest.Status = FriendRequestStatus.Accepted;
            await _context.SaveChangesAsync();

            return new BaseResponseDto<Friend_Request>
            {
                Success = true,
                Message = "Accepted friend request.",
                Data = friendRequest
            };
        }

        public async Task<BaseResponseDto<Friend_Request>> RejectFriendRequestAsync(int sender_id, int reciver_id)
        {
            var friendRequest = await _context.Friend_Requests
                .FirstOrDefaultAsync(fr => fr.Sender_Id == sender_id && fr.Reciver_Id == reciver_id);

            if (friendRequest == null)
            {
                return new BaseResponseDto<Friend_Request>
                {
                    Success = false,
                    Message = "Friend request doesn't exist.",
                    Data = null
                };
            }

            if (friendRequest.Status != FriendRequestStatus.Pending)
            {
                return new BaseResponseDto<Friend_Request>
                {
                    Success = false,
                    Message = "Friend request is not in a pending state.",
                    Data = null
                };
            }

            friendRequest.Status = FriendRequestStatus.Rejected;
            await _context.SaveChangesAsync();

            return new BaseResponseDto<Friend_Request>
            {
                Success = true,
                Message = "Rejected friend request.",
                Data = friendRequest
            };
        }

        public async Task<BaseResponseDto<List<Friend_Request>>> getFriendsAsync(int userId)
        {
            var friendsList = await _context.Friend_Requests
            .Where(fr =>
            fr.Status == FriendRequestStatus.Accepted &&
            (fr.Reciver_Id == userId || fr.Sender_Id == userId))
            .ToListAsync();

            if (friendsList.Count == 0)
            {
                return new BaseResponseDto<List<Friend_Request>>
                {
                    Data = friendsList,
                    Success = false,
                    Message = "no friends."
                };
            }

            return new BaseResponseDto<List<Friend_Request>>
            {
                Data = friendsList,
                Success = true,
                Message = "Friend list retrieved successfully"
            };
        }

        public async Task<BaseResponseDto<Friend_Request>> isFriendAsync(int userId, int checkerId)
        {
            var friend = await _context.Friend_Requests.FirstOrDefaultAsync(fr =>
                fr.Status == FriendRequestStatus.Accepted &&
                (
                    (fr.Sender_Id == userId && fr.Reciver_Id == checkerId) ||
                    (fr.Sender_Id == checkerId && fr.Reciver_Id == userId)
                ));

            if (friend == null)
            {
                return new BaseResponseDto<Friend_Request>
                {
                    Data = null,
                    Success = false,
                    Message = "Users are not friends"
                };
            }

            return new BaseResponseDto<Friend_Request>
            {
                Data = friend,
                Success = true,
                Message = "Users are friends"
            };
        }

    }
}
