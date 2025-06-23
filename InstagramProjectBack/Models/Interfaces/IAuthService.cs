using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
namespace InstagramProjectBack.Repositories
{
    public interface IAuthService
    {
        Task<BaseResponseDto<string>> Register(UserRegisterDto dto);
        Task<BaseResponseDto<string>> Login(UserLoginDto dto);
        Task<bool> VerifyUserAsync(string token);
        Task<BaseResponseDto<string>> ResendVerificationTokenAsync(string email);
        Task<BaseResponseDto<User>> GetUserAsync(int userId);
        Task<BaseResponseDto<User>> GetUserByEmailAsync(string email);
        Task<BaseResponseDto<User>> UpdateUserAsync(int userId,
          string? username = null,
          string? email = null,
          string? password = null,
          string? profileImage = null,
          string? passwordResetToken = null);
        Task<BaseResponseDto<User>> ResetPasswordAsync(int userId, string newPassword);
    }

}
