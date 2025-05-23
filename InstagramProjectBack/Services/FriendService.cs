using InstagramProjectBack.Data;
using InstagramProjectBack.Models.Dto;
namespace InstagramProjectBack.Services
{
    public class FriendService
    {
        private readonly AppDbContext _context;
        public FriendService(AppDbContext context)
        {
            _context = context;
        }

        public bool AreFriends(int senderId, int reciverId)
        {
            bool areFriends = _context.Friend_Requests.Any(f =>
              ((f.Sender_Id == senderId && f.Reciver_Id == reciverId) || (f.Sender_Id == reciverId && f.Reciver_Id == senderId))
              && f.Status == FriendRequestStatus.Accepted);

            return areFriends;
        }
    }
}