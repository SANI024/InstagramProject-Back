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

        public BaseResponseDto<Post> CreatePost(CreatePostDto createPostDto)
        {
            User userExists = _context.Users.FirstOrDefault(u => u.Id == createPostDto.UserId);
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

            Post newPost = new Post
            {
                UserId = createPostDto.UserId,
                VideoUrl = createPostDto.VideoUrl,
                ImageUrl = createPostDto.ImageUrl,
                Description = createPostDto.Description,
                CreatedAt = DateTime.UtcNow
            };

            _context.Posts.Add(newPost);
            _context.SaveChanges();

            return new BaseResponseDto<Post>
            {
                Success = true,
                Data = newPost,
                Message = "Successfully created a post."
            };
        }

        public BaseResponseDto<List<Post>> GetPosts()
        {
            List<Post> postList = _context.Posts.Include(p => p.User).ToList();
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

        public BaseResponseDto<Post> RemovePost(int postId, int userId)
        {
            Post postExists = _context.Posts.FirstOrDefault(p => p.Id == postId && p.UserId == userId);
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
            _context.SaveChanges();

            return new BaseResponseDto<Post>
            {
                Success = true,
                Data = postExists,
                Message = "Post was successfully deleted."
            };
        }

        public BaseResponseDto<Post> UpdatePost(UpdatePostDto updatePostDto)
        {
            Post postExists = _context.Posts.FirstOrDefault(p => p.Id == updatePostDto.PostId && p.UserId == updatePostDto.UserId);
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

            _context.SaveChanges();

            return new BaseResponseDto<Post>
            {
                Success = true,
                Data = postExists,
                Message = "Successfully updated post."
            };
        }
    }
}
