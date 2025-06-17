using InstagramProjectBack.Data;
using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
using InstagramProjectBack.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InstagramProjectBack.Repositories
{
    public class PostLikeRepository : IPostLikeRepository
    {
        private readonly AppDbContext _context; 
        public PostLikeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponseDto<PostLike>> CreatePostLikeAsync(PostLikeRequestDto dto)
        {
            User userExists = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.UserId);
            if (userExists == null)
            {
                return new BaseResponseDto<PostLike>
                {
                    Success = false,
                    Data = null,
                    Message = "User was not found."
                };
            }

            var existingLike = await _context.PostLikes.FirstOrDefaultAsync(pl => pl.PostId == dto.PostId && pl.UserId == dto.UserId);
            if (existingLike != null)
            {
               return new BaseResponseDto<PostLike>
               {
                 Success = false,
                 Data = existingLike,
                 Message = "User already liked this post."
               };
            }
            
            PostLike newPostLike = new PostLike
            {
                PostId = dto.PostId,
                UserId = dto.UserId,
            };

            await _context.PostLikes.AddAsync(newPostLike);
            await _context.SaveChangesAsync();

            return new BaseResponseDto<PostLike> 
            { 
                Success = true,
                Data = newPostLike,
                Message = "Added like"
            };
        }

        public async Task<BaseResponseDto<PostLike>> DeletePostLikeAsync(PostDislikeRequestDto dto)
        {
            User userExists = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.UserId);
            var existing = await _context.PostLikes.FirstOrDefaultAsync(pl => pl.PostId == dto.PostId && pl.UserId == dto.UserId);
            
            if (existing == null)
            {
                return new BaseResponseDto<PostLike> 
                {
                    Success = false,
                    Data = null,
                    Message = "Not liked"
                };
            }

            if (userExists == null)
            {
                return new BaseResponseDto<PostLike>
                {
                    Success = false,
                    Data = null,
                    Message = "User not found"
                };
            }

            _context.PostLikes.Remove(existing);
            await _context.SaveChangesAsync();

            return new BaseResponseDto<PostLike>
            {
                Success = true,
                Data = existing,
                Message = "Like removed"
            };
        }

        public async Task<BaseResponseDto<List<PostLike>>> GetAllPostLikesAsync()
        {
            List<PostLike> likes = await _context.PostLikes.ToListAsync();

            if (likes.Count == 0)
            {
                return new BaseResponseDto<List<PostLike>>
                {
                    Success = false,
                    Data = null,
                    Message = "No posts like available."
                };
            }
             
            return new BaseResponseDto<List<PostLike>>
            {
                Success = true,
                Data = likes,
                Message = "All post likes retrieved."
            };
        }
    }
}
