using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;

namespace InstagramProjectBack.Repositories
{
    public interface IPostRepository
    {
        BaseResponseDto<Post> CreatePost(CreatePostDto createPostDto);
        BaseResponseDto<Post> UpdatePost(UpdatePostDto updatePostDto);
        BaseResponseDto<List<Post>> GetPosts();
        BaseResponseDto<Post> RemovePost(int PostId, int UserId);
    }


}