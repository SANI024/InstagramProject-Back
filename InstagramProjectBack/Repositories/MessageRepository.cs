using InstagramProjectBack.Data;
using InstagramProjectBack.Models;

namespace InstagramProjectBack.Repositories
{

    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;
        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }
        public BaseResponseDto<List<Message>> GetMessages(int UserId)
        {
            User UserExists = _context.Users.FirstOrDefault(u => u.Id == UserId);
            if (UserExists == null)
            {
                return new BaseResponseDto<List<Message>>
                {
                    Success = false,
                    Data = null,
                    Message = "User Doesn't Exists."
                };
            }

            List<Message> UserMessages = _context.Messages.Where(m => m.ReciverId == UserId || m.SenderId == UserId).ToList();
            if (UserMessages.Count == 0)
            {
                return new BaseResponseDto<List<Message>>
                {
                    Success = false,
                    Data = null,
                    Message = "No Messages."
                };
            }

            return new BaseResponseDto<List<Message>>
            {
                Success = true,
                Data = UserMessages,
                Message = "Succesfully Fetched Messages."
            };
        }

        public BaseResponseDto<Message> SendMessage(int senderId, int reciverId, string message)
        {
            User SenderExists = _context.Users.FirstOrDefault(u => u.Id == senderId);
            if (SenderExists == null)
            {
                return new BaseResponseDto<Message>
                {
                    Success = false,
                    Data = null,
                    Message = "Sender Doesn't Exists.",
                };
            }
            User ReciverExists = _context.Users.FirstOrDefault(u => u.Id == reciverId);
            if (ReciverExists == null)
            {
                return new BaseResponseDto<Message>
                {
                    Success = false,
                    Data = null,
                    Message = "Reciver Doesn't Exists.",
                };
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                return new BaseResponseDto<Message>
                {
                    Success = false,
                    Data = null,
                    Message = "Invalid Message Format.",
                };
            }

            Message NewMessage = new Message
            {
                SenderId = senderId,
                ReciverId = reciverId,
                Text = message
            };

            _context.Messages.Add(NewMessage);
            _context.SaveChanges();
            return new BaseResponseDto<Message>
            {
                Success = true,
                Data = NewMessage,
                Message = "Succefully Created Message."
            };
        }
    }
}