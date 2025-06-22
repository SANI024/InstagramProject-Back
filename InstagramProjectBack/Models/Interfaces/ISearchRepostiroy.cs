using InstagramProjectBack.Models;

namespace InstagramProjectBack.Repositories
{
    public interface ISearchRepository
    {
        Task<BaseResponseDto<List<Post>>> SearchPostsAsync(string searchQuery);
        Task<BaseResponseDto<List<User>>> SearchUsersAsync(string searchQuery);
    }
}