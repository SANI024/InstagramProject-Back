using InstagramProjectBack.Models.Dto;

namespace InstagramProjectBack.Repositories
{
    public class PostRepository : IPostRepository
    {
        PostResponseDto IPostRepository.CreatePost(CreatePostDto createPostDto)
        {
            throw new NotImplementedException();
        }

        PostResponseDto IPostRepository.GetPosts()
        {
            throw new NotImplementedException();
        }

        PostResponseDto IPostRepository.RemovePost(int PostId)
        {
            throw new NotImplementedException();
        }

        PostResponseDto IPostRepository.UpdatePost(int PostId)
        {
            throw new NotImplementedException();
        }
    }
}
