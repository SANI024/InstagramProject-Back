using InstagramProjectBack.Models.Dto;

namespace InstagramProjectBack.Repositories
{
    public interface IPostRepository
    {
        PostResponseDto CreatePost(CreatePostDto createPostDto);
        PostResponseDto UpdatePost(int PostId);
        PostResponseDto GetPosts();
        PostResponseDto RemovePost(int PostId);
    }
}