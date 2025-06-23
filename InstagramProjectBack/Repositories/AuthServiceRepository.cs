using InstagramProjectBack.Services;
using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
using Microsoft.AspNetCore.Identity;
using InstagramProjectBack.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Globalization;

namespace InstagramProjectBack.Repositories
{
    public class AuthServiceRepository : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly TokenService _tokenService;
        private readonly EmailService _emailService;
        private readonly VerificationService _verificationService;

        public AuthServiceRepository(
            EmailService emailService,
            VerificationService verificationService,
            AppDbContext context,
            IPasswordHasher<User> passwordHasher,
            TokenService tokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _emailService = emailService;
            _verificationService = verificationService;
        }

        public async Task<BaseResponseDto<User>> GetUserAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new Exception("No user found with the provided email");

            return new BaseResponseDto<User>
            {
                Success = true,
                Message = "Succesfully returned a user",
                Data = user
            };
        }

        public async Task<BaseResponseDto<User>> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new Exception("No user found with the provided email");

            return new BaseResponseDto<User>
            {
                Success = true,
                Message = "Succesfully returned a user",
                Data = user
            };
        }

        public async Task<BaseResponseDto<string>> Register(UserRegisterDto dto)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == dto.Name || u.Email == dto.Email);

            if (existingUser != null)
                throw new Exception("User with provided name or email already exists");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                ProfileImage = dto.ProfileImage,
                IsVerified = false,
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            string verifyToken = _verificationService.GenerateVerifyToken();
            _verificationService.StoreVerifyToken(verifyToken, user.Email);
            await _emailService.SendEmail(user.Email, verifyToken);

            return new BaseResponseDto<string>
            {
                Success = true,
                Message = $"Token Expires At: {user.TokenExpiryDate}",
                Data = null
            };
        }

        public async Task<BaseResponseDto<string>> Login(UserLoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                throw new Exception("Invalid username or password");

            if (!user.IsVerified)
                throw new Exception("User is not verified!");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid username or password");

            var token = _tokenService.CreateToken(user);
            return new BaseResponseDto<string>
            {
                Success = true,
                Message = $"Token Expires At: {user.TokenExpiryDate}",
                Data = token
            };
        }

        public async Task<bool> VerifyUserAsync(string token)
        {
            if (_verificationService.CheckVerifyToken(token, out string email))
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                    return false;

                user.IsVerified = true;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                _verificationService.RemoveVerifyToken(token);
                return true;
            }
            return false;
        }

        public async Task<BaseResponseDto<string>> ResendVerificationTokenAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new Exception("No user found with the provided email");

            if (user.IsVerified)
                throw new Exception("User is already verified");

            string newToken = _verificationService.GenerateVerifyToken();
            _verificationService.StoreVerifyToken(newToken, user.Email);
            await _emailService.SendEmail(user.Email, newToken);

            return new BaseResponseDto<string>
            {
                Success = true,
                Message = "Verification email sent successfully",
                Data = null
            };
        }

        public async Task<BaseResponseDto<User>> UpdateUserAsync(
          int userId,
          string? username = null,
          string? email = null,
          string? password = null,
          string? profileImage = null,
          string? passwordResetToken = null
          )
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(username))
                user.Name = username;

            if (!string.IsNullOrWhiteSpace(email))
                user.Email = email;

            if (!string.IsNullOrWhiteSpace(password))
                user.PasswordHash = _passwordHasher.HashPassword(user, password);

            if (!string.IsNullOrWhiteSpace(profileImage))
                user.ProfileImage = profileImage;

            if (!string.IsNullOrEmpty(passwordResetToken))
            {
                user.PasswordResetToken = passwordResetToken;
                user.PasswordResetTokenCreatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return new BaseResponseDto<User>
            {
                Message = "updated user",
                Success = true,
                Data = user
            };
        }


        public async Task<BaseResponseDto<User>> ResetPasswordAsync(int userId, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return new BaseResponseDto<User>
                {
                    Success = false,
                    Message = "User not found."
                };

            user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenCreatedAt = null;

            await _context.SaveChangesAsync();

            return new BaseResponseDto<User>
            {
                Success = true,
                Message = "Password reset successful.",
                Data = user
            };
        }

    }
}
