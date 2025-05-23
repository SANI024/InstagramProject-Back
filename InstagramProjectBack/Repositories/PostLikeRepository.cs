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
        public BaseResponseDto<PostLike> CreatePostLike(PostLike postLike)
        {
            User userExists = _context.Users.FirstOrDefault(u => u.Id == postLike.UserId);
            if (userExists == null)
            {
                return new BaseResponseDto<PostLike>
                {
                    Success = false,
                    Data = null,
                    Message = "User was not found."
                };
            }
            PostLike NewPostLike = new PostLike
            {
                PostId = postLike.PostId,
                UserId = postLike.UserId,

            };
            _context.PostLikes.Add(NewPostLike);
            _context.SaveChanges();

            return new BaseResponseDto<PostLike> 
            { 
                Success= true,
                Data= NewPostLike,
                Message= "added like"
            };

        }

        public BaseResponseDto<PostLike> DeletePostLike(PostLike postLike)
        {
            User userExists = _context.Users.FirstOrDefault(u => u.Id == postLike.UserId);
            PostLike existing = _context.PostLikes.FirstOrDefault(pl => pl.Id == postLike.Id);


            if (existing == null )
            {
                return new BaseResponseDto<PostLike> 
                {
                    Success = false,
                    Data = null,
                    Message="not liked"
                };

            }

            if(userExists == null)
            {
                return new BaseResponseDto<PostLike>
                {
                    Success = false,
                    Data = null,
                    Message = "user not found"
                };
            }
             _context.PostLikes .Remove(existing);
            _context.SaveChanges();

            return new BaseResponseDto<PostLike>
            {
                Success = true,
                Data = existing,
                Message = "like removed"
            };

        }

        public BaseResponseDto<List<PostLike>> GetAllPostLikes()
        {
            List<PostLike> likes = _context.PostLikes
            .Include(pl => pl.Post)
            .Include(pl => pl.User)
            .ToList();

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
