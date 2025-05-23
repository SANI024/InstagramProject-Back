namespace InstagramProjectBack.Models.Interfaces
{
    public interface IPostLikeRepository
    {
        BaseResponseDto<List<PostLike>> GetAllPostLikes();
        BaseResponseDto<PostLike> CreatePostLike(PostLike postLike);
        BaseResponseDto<PostLike> DeletePostLike(PostLike postLike);
       
    }
}
