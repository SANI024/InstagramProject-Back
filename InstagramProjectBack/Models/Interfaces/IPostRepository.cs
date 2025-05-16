using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;

namespace InstagramProjectBack.Repositories
{
    public interface IPostRepository
    {
        PostResponseDto<Post> CreatePost(CreatePostDto createPostDto);
        PostResponseDto<Post> UpdatePost(UpdatePostDto updatePostDto);
        PostResponseDto<List<Post>> GetPosts();
        PostResponseDto<Post> RemovePost(int PostId, int UserId);
    }


}