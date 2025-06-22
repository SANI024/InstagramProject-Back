using InstagramProjectBack.Data;
using InstagramProjectBack.Models;

namespace InstagramProjectBack.Repositories
{
    public class SearchRepository : ISearchRepository
    {

        private readonly AppDbContext _context;

        public SearchRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task<BaseResponseDto<List<Post>>> SearchPostsAsync(string searchQuery)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseDto<List<User>>> SearchUsersAsync(string searchQuery)
        {
            throw new NotImplementedException();
        }
    }
}