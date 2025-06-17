using InstagramProjectBack.Data;
using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace InstagramProjectBack.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponseDto<Post>> CreatePostAsync(CreatePostDto createPostDto)
        {
            var userExists = await _context.Users.FirstOrDefaultAsync(u => u.Id == createPostDto.UserId);
            if (userExists == null)
            {
                return new BaseResponseDto<Post>
                {
                    Success = false,
                    Data = null,
                    Message = "User was not found."
                };
            }

            if (string.IsNullOrWhiteSpace(createPostDto.Description))
            {
                return new BaseResponseDto<Post>
                {
                    Success = false,
                    Data = null,
                    Message = "Description cannot be null."
                };
            }

            var newPost = new Post
            {
                UserId = createPostDto.UserId,
                VideoUrl = createPostDto.VideoUrl,
                ImageUrl = createPostDto.ImageUrl,
                Description = createPostDto.Description,
                CreatedAt = DateTime.UtcNow
            };

            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();

            return new BaseResponseDto<Post>
            {
                Success = true,
                Data = newPost,
                Message = "Successfully created a post."
            };
        }

        public async Task<BaseResponseDto<List<Post>>> GetPostsAsync()
        {
            var postList = await _context.Posts
                .Include(p => p.User)
                .ToListAsync();

            if (postList.Count == 0)
            {
                return new BaseResponseDto<List<Post>>
                {
                    Success = false,
                    Data = null,
                    Message = "No posts available."
                };
            }

            return new BaseResponseDto<List<Post>>
            {
                Success = true,
                Data = postList,
                Message = "Successfully fetched posts."
            };
        }

        public async Task<BaseResponseDto<Post>> RemovePostAsync(int postId, int userId)
        {
            var postExists = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId && p.UserId == userId);
            if (postExists == null)
            {
                return new BaseResponseDto<Post>
                {
                    Success = false,
                    Data = null,
                    Message = "Post was not found."
                };
            }

            _context.Posts.Remove(postExists);
            await _context.SaveChangesAsync();

            return new BaseResponseDto<Post>
            {
                Success = true,
                Data = postExists,
                Message = "Post was successfully deleted."
            };
        }

        public async Task<BaseResponseDto<Post>> UpdatePostAsync(UpdatePostDto updatePostDto)
        {
            var postExists = await _context.Posts.FirstOrDefaultAsync(p => p.Id == updatePostDto.PostId && p.UserId == updatePostDto.UserId);
            if (postExists == null)
            {
                return new BaseResponseDto<Post>
                {
                    Success = false,
                    Data = null,
                    Message = "Post was not found."
                };
            }

            if (!string.IsNullOrWhiteSpace(updatePostDto.ImageUrl))
            {
                postExists.ImageUrl = updatePostDto.ImageUrl;
            }

            if (!string.IsNullOrWhiteSpace(updatePostDto.VideoUrl))
            {
                postExists.VideoUrl = updatePostDto.VideoUrl;
            }

            if (!string.IsNullOrWhiteSpace(updatePostDto.Description))
            {
                postExists.Description = updatePostDto.Description;
            }

            await _context.SaveChangesAsync();

            return new BaseResponseDto<Post>
            {
                Success = true,
                Data = postExists,
                Message = "Successfully updated post."
            };
        }
    }
}
