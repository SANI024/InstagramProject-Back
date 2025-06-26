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

        public async Task<BaseResponseDto<Friend_RequestDto>> SendFriendRequestAsync(int sender_id, int reciver_id)
        {
            if (sender_id == reciver_id)
            {
                return new BaseResponseDto<Friend_RequestDto>
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
                return new BaseResponseDto<Friend_RequestDto>
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

            var newFriendRequestDto = new Friend_RequestDto
            {
                Sender_Id = sender_id,
                Reciver_Id = reciver_id,
                Status = FriendRequestStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            return new BaseResponseDto<Friend_RequestDto>
            {
                Success = true,
                Message = "Friend request sent successfully.",
                Data = newFriendRequestDto
            };
        }

        public async Task<BaseResponseDto<List<Friend_RequestDto>>> GetFriendRequestsByReciverIdAsync(int reciver_id)
        {
            var friendRequests = await _context.Friend_Requests
                .Include(fr => fr.Sender)
                .Where(fr => fr.Reciver_Id == reciver_id && fr.Status == FriendRequestStatus.Pending)
                .ToListAsync();

            if (friendRequests.Count == 0)
            {
                return new BaseResponseDto<List<Friend_RequestDto>>
                {
                    Success = false,
                    Message = "No requests.",
                    Data = null
                };
            }

            var friendRequestDtos = friendRequests.Select(fr => new Friend_RequestDto
            {
                Id = fr.Id,
                Sender_Id = fr.Sender_Id,
                Reciver_Id = fr.Reciver_Id,
                Status = fr.Status,
                CreatedAt = fr.CreatedAt,
                Sender = new UserDto
                {
                    Id = fr.Sender.Id,
                    Name = fr.Sender.Name,
                    Email = fr.Sender.Email,
                    ProfileImage = fr.Sender.ProfileImage
                },
                Reciver = new UserDto
                {
                    Id = fr.Reciver.Id,
                    Name = fr.Reciver.Name,
                    Email = fr.Reciver.Email,
                    ProfileImage = fr.Sender.ProfileImage
                }
            }).ToList();


            return new BaseResponseDto<List<Friend_RequestDto>>
            {
                Success = true,
                Message = "Successfully fetched friend requests.",
                Data = friendRequestDtos
            };
        }

        public async Task<BaseResponseDto<Friend_RequestDto>> AcceptFriendRequestAsync(int sender_id, int reciver_id)
        {
            var friendRequest = await _context.Friend_Requests
                .FirstOrDefaultAsync(fr => fr.Sender_Id == sender_id && fr.Reciver_Id == reciver_id);

            if (friendRequest == null)
            {
                return new BaseResponseDto<Friend_RequestDto>
                {
                    Success = false,
                    Message = "Friend request doesn't exist.",
                    Data = null
                };
            }

            if (friendRequest.Status != FriendRequestStatus.Pending)
            {
                return new BaseResponseDto<Friend_RequestDto>
                {
                    Success = false,
                    Message = "Friend request is not in a pending state.",
                    Data = null
                };
            }

            friendRequest.Status = FriendRequestStatus.Accepted;
            await _context.SaveChangesAsync();

            var friendRequestDto = new Friend_RequestDto
            {
                Id = friendRequest.Id,
                Sender_Id = friendRequest.Sender_Id,
                Reciver_Id = friendRequest.Reciver_Id,
                Status = friendRequest.Status,
                CreatedAt = friendRequest.CreatedAt,
                Sender = new UserDto
                {
                    Id = friendRequest.Sender.Id,
                    Name = friendRequest.Sender.Name,
                    Email = friendRequest.Sender.Email,
                    ProfileImage = friendRequest.Sender.ProfileImage
                },
                Reciver = new UserDto
                {
                    Id = friendRequest.Reciver.Id,
                    Name = friendRequest.Reciver.Name,
                    Email = friendRequest.Reciver.Email,
                    ProfileImage = friendRequest.Sender.ProfileImage
                }
            };

            return new BaseResponseDto<Friend_RequestDto>
            {
                Success = true,
                Message = "Accepted friend request.",
                Data = friendRequestDto
            };
        }

        public async Task<BaseResponseDto<Friend_RequestDto>> RejectFriendRequestAsync(int sender_id, int reciver_id)
        {
            var friendRequest = await _context.Friend_Requests
                .FirstOrDefaultAsync(fr => fr.Sender_Id == sender_id && fr.Reciver_Id == reciver_id);

            if (friendRequest == null)
            {
                return new BaseResponseDto<Friend_RequestDto>
                {
                    Success = false,
                    Message = "Friend request doesn't exist.",
                    Data = null
                };
            }

            if (friendRequest.Status != FriendRequestStatus.Pending)
            {
                return new BaseResponseDto<Friend_RequestDto>
                {
                    Success = false,
                    Message = "Friend request is not in a pending state.",
                    Data = null
                };
            }

            friendRequest.Status = FriendRequestStatus.Rejected;
            await _context.SaveChangesAsync();

            var friendRequestDto = new Friend_RequestDto
            {
                Id = friendRequest.Id,
                Sender_Id = friendRequest.Sender_Id,
                Reciver_Id = friendRequest.Reciver_Id,
                Status = friendRequest.Status,
                CreatedAt = friendRequest.CreatedAt,
                Sender = new UserDto
                {
                    Id = friendRequest.Sender.Id,
                    Name = friendRequest.Sender.Name,
                    Email = friendRequest.Sender.Email,
                    ProfileImage = friendRequest.Sender.ProfileImage
                },
                Reciver = new UserDto
                {
                    Id = friendRequest.Reciver.Id,
                    Name = friendRequest.Reciver.Name,
                    Email = friendRequest.Reciver.Email,
                    ProfileImage = friendRequest.Sender.ProfileImage
                }
            };

            return new BaseResponseDto<Friend_RequestDto>
            {
                Success = true,
                Message = "Rejected friend request.",
                Data = friendRequestDto
            };
        }

        public async Task<BaseResponseDto<List<UserDto>>> getFriendsAsync(int userId)
        {
            var friendsList = await _context.Friend_Requests
            .Include(fr => fr.Sender)
            .Include(fr => fr.Reciver)
            .Where(fr =>
            fr.Status == FriendRequestStatus.Accepted &&
            (fr.Reciver_Id == userId || fr.Sender_Id == userId))
            .Select(fr => fr.Reciver_Id == userId ? fr.Sender : fr.Reciver)
            .ToListAsync();



            if (friendsList.Count == 0)
            {
                return new BaseResponseDto<List<UserDto>>
                {
                    Data = null,
                    Success = false,
                    Message = "no friends."
                };
            }

            var friendDtos = friendsList.Select(friend => new UserDto
            {
                Id = friend.Id,
                Name = friend.Name,
                Email = friend.Email,
                ProfileImage = friend.ProfileImage
            }).ToList();

            return new BaseResponseDto<List<UserDto>>
            {
                Data = friendDtos,
                Success = true,
                Message = "Friend list retrieved successfully"
            };
        }

        public async Task<BaseResponseDto<Friend_RequestDto>> GetFriendRequestStatusAsync(int userId, int checkerId)
        {
            var friend = await _context.Friend_Requests
            .Include(fr => fr.Sender)
            .Include(fr => fr.Reciver)
            .FirstOrDefaultAsync(fr =>
                (
                    (fr.Sender_Id == userId && fr.Reciver_Id == checkerId) ||
                    (fr.Sender_Id == checkerId && fr.Reciver_Id == userId)
                ));

            if (friend.Status == FriendRequestStatus.Pending)
            {
                return new BaseResponseDto<Friend_RequestDto>
                {
                    Data = null,
                    Success = false,
                    Message = "Pending."
                };
            }

            if (friend.Status == FriendRequestStatus.Rejected)
            {
                return new BaseResponseDto<Friend_RequestDto>
                {
                    Data = null,
                    Success = false,
                    Message = "Rejected."
                };
            }

            if (friend == null)
            {
                return new BaseResponseDto<Friend_RequestDto>
                {
                    Data = null,
                    Success = false,
                    Message = "Users are not friends."
                };
            }

            var FriendRequestDto = new Friend_RequestDto
            {

                Id = friend.Id,
                Sender_Id = friend.Sender_Id,
                Reciver_Id = friend.Reciver_Id,
                Status = friend.Status,
                CreatedAt = friend.CreatedAt,
                Sender = new UserDto
                {
                    Id = friend.Sender.Id,
                    Name = friend.Sender.Name,
                    Email = friend.Sender.Email,
                    ProfileImage = friend.Sender.ProfileImage
                },
                Reciver = new UserDto
                {
                    Id = friend.Reciver.Id,
                    Name = friend.Reciver.Name,
                    Email = friend.Reciver.Email,
                    ProfileImage = friend.Sender.ProfileImage
                }
            };

            return new BaseResponseDto<Friend_RequestDto>
            {
                Data = FriendRequestDto,
                Success = true,
                Message = "Users are friends"
            };
        }

    }
}
