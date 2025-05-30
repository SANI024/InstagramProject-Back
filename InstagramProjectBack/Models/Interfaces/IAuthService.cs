
using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;

namespace InstagramProjectBack.Repositories
{
    public interface IAuthService
    {
        Task<BaseResponseDto<string>> Register(UserRegisterDto dto);
        BaseResponseDto<string> Login(UserLoginDto dto);
        Task<BaseResponseDto<string>> ResendVerificationTokenAsync(string email);
        bool VerifyUser(string token);
    }
}
