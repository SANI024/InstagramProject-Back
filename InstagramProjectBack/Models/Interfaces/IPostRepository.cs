using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;

namespace InstagramProjectBack.Repositories
{
    public interface IPostRepository
    {
        Task<BaseResponseDto<Post>> CreatePostAsync(CreatePostDto createPostDto);
        Task<BaseResponseDto<Post>> UpdatePostAsync(UpdatePostDto updatePostDto);
        Task<BaseResponseDto<List<Post>>> GetPostsAsync();
        Task<BaseResponseDto<Post>> GetPostAsync(int postId);
        Task<BaseResponseDto<Post>> RemovePostAsync(int postId, int userId);
    }
}
