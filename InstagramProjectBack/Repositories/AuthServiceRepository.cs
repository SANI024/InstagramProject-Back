using InstagramProjectBack.Services;
using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
using Microsoft.AspNetCore.Identity;
using InstagramProjectBack.Data;
using System.Threading.Tasks;

namespace InstagramProjectBack.Repositories
{
    public class AuthServiceRepository : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly TokenService _tokenService;
        private readonly EmailService _emailService;
        private readonly VerificationService _verificationService;

        public AuthServiceRepository(EmailService emailService, VerificationService verificationService, AppDbContext context, IPasswordHasher<User> passwordHasher, TokenService tokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _emailService = emailService;
            _verificationService = verificationService;
        }

        public async Task<BaseResponseDto<string>> Register(UserRegisterDto dto)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Name == dto.Name || u.Email == dto.Email);
            if (existingUser != null)
                throw new Exception("User with provided name or email already exists");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PofileImage = dto.ProfileImage,
                IsVerified = false
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
            string VerifyToken = _verificationService.GenerateVerifyToken();
            _verificationService.StoreVerifyToken(VerifyToken, user.Email);
            await _emailService.SendEmail(user.Email,VerifyToken);
            return new BaseResponseDto<string>
            {
                Success = true,
                Message = $"Token Expires At: {user.TokenExpiryDate}",
                Data = null
            };
        }

        public BaseResponseDto<string> Login(UserLoginDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null)
                throw new Exception("Invalid username or password");

            if (user.IsVerified == false)
                throw new Exception("user is not verified!");

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

        public bool VerifyUser(string token)
        {
            if (_verificationService.CheckVerifyToken(token, out string email))
            {
                User user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    return false;
                }

                user.IsVerified = true;
                _context.Users.Update(user);
                _context.SaveChanges();
                _verificationService.RemoveVerifyToken(token);
                return true;
            }
            return false;       
        }

        public async Task<BaseResponseDto<string>> ResendVerificationTokenAsync(string email)
        {
          var user = _context.Users.FirstOrDefault(u => u.Email == email);
          if (user == null)
           throw new Exception("No user found with the provided email");

          if (user.IsVerified)
           throw new Exception("User is already verified");

          string NewToken = _verificationService.GenerateVerifyToken();
          _verificationService.StoreVerifyToken(NewToken, user.Email);
          await _emailService.SendEmail(user.Email,NewToken);

          return new BaseResponseDto<string>
          {
            Success = true,
            Message = "Verification email sent successfully",
            Data = null
          };
        }
    }
}
