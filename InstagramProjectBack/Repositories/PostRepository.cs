using AutoMapper;
using InstagramProjectBack.Data;
using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace InstagramProjectBack.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PostRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<BaseResponseDto<Post>> GetPostAsync(int postId)
        {
            Post post = _context.Posts
            .Include(p => p.User)
            .Include(p => p.Comments)
            .Include(p => p.Likes)
            .FirstOrDefault(p => p.Id == postId);

            if (post == null)
            {
                return new BaseResponseDto<Post>
                {
                    Success = false,
                    Data = null,
                    Message = "post doesnt exists."
                };
            }

            return new BaseResponseDto<Post>
            {
                Data = post,
                Success = true,
                Message = "Succesfully fetched post."

            };


        }

        public async Task<BaseResponseDto<List<PostDto>>> GetPostsAsync()
        {
            var postList = await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Comments)
                .ThenInclude(c => c.User)
            .Include(p => p.Likes)
                .ThenInclude(l => l.User)
            .ToListAsync();

            var postDtos = _mapper.Map<List<PostDto>>(postList);

            if (postList.Count == 0)
            {
                return new BaseResponseDto<List<PostDto>>
                {
                    Success = false,
                    Data = null,
                    Message = "No posts available."
                };
            }

            return new BaseResponseDto<List<PostDto>>
            {
                Success = true,
                Data = postDtos,
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
        public async Task<BaseResponseDto<List<Post>>> GetLikedPostsAsync(int userId)
        {
            var likedPosts = await _context.Posts
             .Include(p => p.Likes)
             .Where(p => p.Likes.Any(like => like.UserId == userId))
             .ToListAsync();
            if (likedPosts.Count == 0)
            {
                return new BaseResponseDto<List<Post>>
                {
                  Success = false,
                  Data = null,
                  Message = "no liked posts."
                }; 
            }
            return new BaseResponseDto<List<Post>>
            {
              Success = true,
              Data = likedPosts,
              Message = "Succesfully returned Liked posts."
            };
        }

        public async Task<BaseResponseDto<List<Post>>> GetCreatedPostByUser(int userId)
        {
            var createdPostsByUser = await _context.Posts
        .Where(p => p.UserId == userId)
        .Include(p => p.User)
        .Include(p => p.Likes)
        .Include(p => p.Comments)
        .OrderByDescending(p => p.CreatedAt)
        .ToListAsync();

           if(createdPostsByUser.Count == 0)
            {
                return new BaseResponseDto<List<Post>>
                {
                    Success = false,
                    Data = null,
                    Message = "no posts created by this user."
                };
            }
            return new BaseResponseDto<List<Post>>
            {
                Success = true,
                Data = createdPostsByUser,
                Message = "Succesfully returned posts created by this user."
            };
        }
    }
}
