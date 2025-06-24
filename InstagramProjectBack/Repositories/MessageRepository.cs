using InstagramProjectBack.Data;
using InstagramProjectBack.Models;

using Microsoft.EntityFrameworkCore;

namespace InstagramProjectBack.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponseDto<List<Message>>> GetMessagesAsync(int loggedInUserId, int userId)
        {
            var userExists = await _context.Users.FirstOrDefaultAsync(u => u.Id == loggedInUserId);
            if (userExists == null)
            {
                return new BaseResponseDto<List<Message>>
                {
                    Success = false,
                    Data = null,
                    Message = "User doesn't exist."
                };
            }

            var messages = await _context.Messages
             .Where(m =>
            (m.SenderId == loggedInUserId && m.ReciverId == userId) ||
            (m.SenderId == userId && m.ReciverId == loggedInUserId))
            .ToListAsync();

            if (messages.Count == 0)
            {
                return new BaseResponseDto<List<Message>>
                {
                    Success = false,
                    Data = null,
                    Message = "No messages found."
                };
            }

            return new BaseResponseDto<List<Message>>
            {
                Success = true,
                Data = messages,
                Message = "Successfully fetched messages."
            };
        }

        public async Task<BaseResponseDto<Message>> SendMessageAsync(int senderId, int reciverId, string message)
        {
            var senderExists = await _context.Users.FindAsync(senderId);
            if (senderExists == null)
            {
                return new BaseResponseDto<Message>
                {
                    Success = false,
                    Data = null,
                    Message = "Sender doesn't exist."
                };
            }

            var reciverExists = await _context.Users.FindAsync(reciverId);
            if (reciverExists == null)
            {
                return new BaseResponseDto<Message>
                {
                    Success = false,
                    Data = null,
                    Message = "Receiver doesn't exist."
                };
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                return new BaseResponseDto<Message>
                {
                    Success = false,
                    Data = null,
                    Message = "Invalid message format."
                };
            }

            var newMessage = new Message
            {
                SenderId = senderId,
                ReciverId = reciverId,
                Text = message
            };

            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();

            return new BaseResponseDto<Message>
            {
                Success = true,
                Data = newMessage,
                Message = "Successfully created message."
            };
        }
    }
}
