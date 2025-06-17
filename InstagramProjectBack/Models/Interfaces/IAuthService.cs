using InstagramProjectBack.Models.Dto;
namespace InstagramProjectBack.Repositories
{
    public interface IAuthService
    {
        Task<BaseResponseDto<string>> Register(UserRegisterDto dto);
        Task<BaseResponseDto<string>> Login(UserLoginDto dto);
        Task<bool> VerifyUserAsync(string token);
        Task<BaseResponseDto<string>> ResendVerificationTokenAsync(string email);
    }
}
