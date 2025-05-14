using InstagramProjectBack.Data;
using InstagramProjectBack.Models;
using InstagramProjectBack.Services;


namespace InstagramProjectBack.Repositories
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public FriendRequestRepository(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public FriendRequestResponseDto SendFriendRequest(int sender_id, int reciver_id)
        {
            Friend_Request FriendRequestExists = _context.Friend_Requests.FirstOrDefault(fr => fr.Sender_Id == sender_id && fr.Reciver_Id == reciver_id);
            if (FriendRequestExists != null)
            {
                return new FriendRequestResponseDto { Success = false, Message = "Friend request already sent." };
            }
            Friend_Request NewFriendRequest = new Friend_Request
            {
                Sender_Id = sender_id,
                Reciver_Id = reciver_id,
                Status = FriendRequestStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            _context.Friend_Requests.Add(NewFriendRequest);
            _context.SaveChanges();
            return new FriendRequestResponseDto { Success = true, Message = "Friend request sent successfully." };
        }
        public FriendRequestResponseDto GetFriendRequestsByReciverId(int reciver_id)
        {
            List<Friend_Request> FriendRequests = _context.Friend_Requests
            .Where(fr => fr.Reciver_Id == reciver_id && fr.Status == FriendRequestStatus.Pending).ToList();
            if (FriendRequests.Count == 0)
            {
                return new FriendRequestResponseDto { Success = false, Message = "No requests." };
            }
            return new FriendRequestResponseDto { Success = true, Friend_Requests = FriendRequests };
        }


        public FriendRequestResponseDto AcceptFriendRequest(int sender_id, int reciver_id)
        {
            Friend_Request FriendRequest = _context.Friend_Requests.FirstOrDefault(fr => fr.Sender_Id == sender_id && fr.Reciver_Id == reciver_id);
            if (FriendRequest == null)
            {
                return new FriendRequestResponseDto { Success = false, Message = "Friend request doesn't exists." };
            }

            if (FriendRequest.Status != FriendRequestStatus.Pending)
            {
                return new FriendRequestResponseDto { Success = false, Message = "Friend request is not in a pending state." };
            }

            FriendRequest.Status = FriendRequestStatus.Accepted;
            _context.SaveChanges();
            return new FriendRequestResponseDto { Success = true, Message = "Accepted friend request." };
        }

        public FriendRequestResponseDto RejectFriendRequest(int sender_id, int reciver_id)
        {
            Friend_Request FriendRequest = _context.Friend_Requests.FirstOrDefault(fr => fr.Sender_Id == sender_id && fr.Reciver_Id == reciver_id);
            if (FriendRequest == null)
            {
                return new FriendRequestResponseDto { Success = false, Message = "Friend request doesn't exists." };
            }

            if (FriendRequest.Status != FriendRequestStatus.Pending)
            {
                return new FriendRequestResponseDto { Success = false, Message = "Friend request is not in a pending state." };
            }

            FriendRequest.Status = FriendRequestStatus.Rejected;
            _context.SaveChanges();
            return new FriendRequestResponseDto { Success = true, Message = "Rejected friend request." };
        }
    }
}