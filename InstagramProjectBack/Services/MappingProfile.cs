using AutoMapper;

using InstagramProjectBack.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<Post, PostDto>();
        CreateMap<PostComment, PostCommentDto>();
        CreateMap<PostLike, PostLikeDto>();
        CreateMap<Friend_Request, Friend_RequestDto>();
    }
}