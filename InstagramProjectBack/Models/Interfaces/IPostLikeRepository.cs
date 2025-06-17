using InstagramProjectBack.Models.Dto;

namespace InstagramProjectBack.Models.Interfaces
{
    public interface IPostLikeRepository
    {
        Task<BaseResponseDto<List<PostLike>>> GetAllPostLikesAsync();
        Task<BaseResponseDto<PostLike>> CreatePostLikeAsync(PostLikeRequestDto dto);
        Task<BaseResponseDto<PostLike>> DeletePostLikeAsync(PostDislikeRequestDto dto);
    }
}
