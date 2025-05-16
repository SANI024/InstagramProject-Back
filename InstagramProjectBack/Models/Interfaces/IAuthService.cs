using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;

namespace InstagramProjectBack.Repositories
{
    public interface IAuthService
    {
        BaseResponseDto<string> Register(UserRegisterDto dto);
        BaseResponseDto<string> Login(UserLoginDto dto);
    }
}
