using AutoMapper;

using InstagramProjectBack.Data;
using InstagramProjectBack.Models;

using Microsoft.EntityFrameworkCore;

using Org.BouncyCastle.Pqc.Crypto.Falcon;

namespace InstagramProjectBack.Repositories
{
    public class SearchRepository : ISearchRepository
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SearchRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<BaseResponseDto<List<UserDto>>> SearchUsersAsync(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return new BaseResponseDto<List<UserDto>>
                {
                    Data = null,
                    Message = "Search query is empty.",
                    Success = false
                };
            }

            var usersList = await _context.Users
                .Where(u => u.Name.ToLower().Contains(searchQuery.ToLower()))
                .ToListAsync();

            if (usersList.Count == 0)
            {
                return new BaseResponseDto<List<UserDto>>
                {
                    Data = null,
                    Message = "Users not found",
                    Success = false
                };
            }

            var userDtos = _mapper.Map<List<UserDto>>(usersList);

            return new BaseResponseDto<List<UserDto>>
            {
                Data = userDtos,
                Message = "Users found.",
                Success = true
            };
        }



        public async Task<BaseResponseDto<List<PostDto>>> SearchPostsAsync(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return new BaseResponseDto<List<PostDto>>
                {
                    Data = null,
                    Message = "Search query is empty.",
                    Success = false
                };
            }

            var postList = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Where(p => p.Description.ToLower().Contains(searchQuery.ToLower()) || p.User.Name.ToLower().Contains(searchQuery.ToLower()))
                .ToListAsync();

            if (postList.Count == 0)
            {
                return new BaseResponseDto<List<PostDto>>
                {
                    Data = null,
                    Message = "Posts not found",
                    Success = false
                };
            }

            var postDtos = _mapper.Map<List<PostDto>>(postList);

            return new BaseResponseDto<List<PostDto>>
            {
                Data = postDtos,
                Message = "Posts found",
                Success = true
            };
        }


    }
}