using InstagramProjectBack.Services;
using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
using Microsoft.AspNetCore.Identity;
using InstagramProjectBack.Data;

namespace InstagramProjectBack.Repositories
{
    public class AuthServiceRepository : IAuthService
    {

        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly TokenService _tokenService;

        public AuthServiceRepository(AppDbContext context, IPasswordHasher<User> passwordHasher, TokenService tokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public AuthResponceDto Register(UserRegisterDto dto)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Name == dto.Name);
            if (existingUser != null)
                throw new Exception("User already exists");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PofileImage = dto.ProfileImage
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            _context.Users.Add(user);
            _context.SaveChanges();

            var token = _tokenService.CreateToken(user);
            return new AuthResponceDto { Token = token, ExpiresAt = user.TokenExpiryDate };
        }

        public AuthResponceDto Login(UserLoginDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null)
                throw new Exception("Invalid username or password");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid username or password");

            var token = _tokenService.CreateToken(user);
            return new AuthResponceDto { Token = token, ExpiresAt = user.TokenExpiryDate };
        }


    }
}
