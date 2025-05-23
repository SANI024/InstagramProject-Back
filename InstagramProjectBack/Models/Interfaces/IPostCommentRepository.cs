using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;

namespace InstagramProjectBack.Repositories
{
    public interface IPostCommentRepository
    {
        BaseResponseDto<PostComment> CreatePostComment(CreatePostCommentDto dto);
        BaseResponseDto<PostComment> RemovePostComment(int UserId, int PostCommentId);
        BaseResponseDto<PostComment> UpdatePostComment(UpdatePostCommentDto dto);
        BaseResponseDto<List<PostComment>> GetPostComments();
    }
}
