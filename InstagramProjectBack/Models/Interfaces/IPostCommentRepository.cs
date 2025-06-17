using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;


namespace InstagramProjectBack.Repositories
{
    public interface IPostCommentRepository
    {
        Task<BaseResponseDto<PostComment>> CreatePostCommentAsync(CreatePostCommentDto dto);
        Task<BaseResponseDto<PostComment>> RemovePostCommentAsync(int userId, int postCommentId);
        Task<BaseResponseDto<PostComment>> UpdatePostCommentAsync(UpdatePostCommentDto dto);
        Task<BaseResponseDto<List<PostComment>>> GetPostCommentsAsync();
    }
}
