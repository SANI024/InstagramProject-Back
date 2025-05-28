using InstagramProjectBack.Models.Dto;

namespace InstagramProjectBack.Models.Interfaces
{
    public interface IPostLikeRepository
    {
        BaseResponseDto<List<PostLike>> GetAllPostLikes();
        BaseResponseDto<PostLike> CreatePostLike(PostLikeRequestDto dto);
        BaseResponseDto<PostLike> DeletePostLike(PostLike postLike);
       
    }
}
