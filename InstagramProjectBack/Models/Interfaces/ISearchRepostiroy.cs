using InstagramProjectBack.Models;

namespace InstagramProjectBack.Repositories
{
    public interface ISearchRepository
    {
        Task<BaseResponseDto<List<UserDto>>> SearchUsersAsync(string searchQuery);
        Task<BaseResponseDto<List<PostDto>>> SearchPostsAsync(string searchQuery);
    }
}