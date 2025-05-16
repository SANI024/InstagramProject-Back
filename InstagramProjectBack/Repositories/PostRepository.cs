using InstagramProjectBack.Data;
using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;

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
            try
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
            catch (Exception ex)
            {
                return new BaseResponseDto<Post>
                {
                    Success = false,
                    Data = null,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }


        BaseResponseDto<List<Post>> IPostRepository.GetPosts()
        {
            try
            {
                List<Post> PostList = _context.Posts.ToList();
                if (PostList.Count == 0)
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
                    Data = PostList,
                    Message = "Successfully fetched posts."
                };
            }
            catch (Exception ex)
            {

                return new BaseResponseDto<List<Post>>
                {
                    Success = false,
                    Data = null,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        BaseResponseDto<Post> IPostRepository.RemovePost(int PostId, int UserId)
        {
            try
            {
                Post PostExists = _context.Posts.FirstOrDefault(p => p.Id == PostId && p.UserId == UserId);
                if (PostExists == null)
                {
                    return new BaseResponseDto<Post>
                    {
                        Success = false,
                        Data = null,
                        Message = $"Post was not found."
                    };
                }
                _context.Posts.Remove(PostExists);
                _context.SaveChanges();
                return new BaseResponseDto<Post>
                {
                    Success = true,
                    Data = PostExists,
                    Message = $"Post was succesfully deleted."
                };
            }
            catch (Exception ex)
            {

                return new BaseResponseDto<Post>
                {
                    Success = false,
                    Data = null,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        BaseResponseDto<Post> IPostRepository.UpdatePost(UpdatePostDto updatePostDto)
        {
            try
            {
                Post PostExists = _context.Posts.FirstOrDefault(p => p.Id == updatePostDto.PostId && p.UserId == updatePostDto.UserId);
                if (PostExists == null)
                {
                    return new BaseResponseDto<Post>
                    {
                        Success = false,
                        Data = null,
                        Message = $"Post was not found."
                    };
                }
                if (!string.IsNullOrWhiteSpace(updatePostDto.ImageUrl))
                {
                    PostExists.ImageUrl = updatePostDto.ImageUrl;
                }
                if (!string.IsNullOrWhiteSpace(updatePostDto.VideoUrl))
                {
                    PostExists.VideoUrl = updatePostDto.VideoUrl;
                }
                if (!string.IsNullOrWhiteSpace(updatePostDto.Description))
                {
                    PostExists.Description = updatePostDto.Description;
                }
                _context.SaveChanges();
                return new BaseResponseDto<Post>
                {
                    Success = true,
                    Data = PostExists,
                    Message = "Succesfully updated post."
                };

            }
            catch (Exception ex)
            {

                return new BaseResponseDto<Post>
                {
                    Success = false,
                    Data = null,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
}
