using InstagramProjectBack.Models.Dto;

namespace InstagramProjectBack.Repositories
{
    public interface IAuthService
    {
        AuthResponceDto Register(UserRegisterDto dto);
        AuthResponceDto Login(UserLoginDto dto);
    }
}
