using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InstagramProjectBack.Models;
using Microsoft.IdentityModel.Tokens;


namespace InstagramProjectBack.Services
{
    
    public class TokenService
    {

        private readonly IConfiguration configuration;
        private readonly ILogger<TokenService> logger;

        public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }


        public string CreateToken(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]!));

            logger.LogInformation($"key: {key}");

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            logger.LogInformation($"creds: {creds}");

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration["AppSettings:Issuer"],
                audience: configuration["AppSettings:Audience"],
                claims: claims,
                signingCredentials: creds,
                expires: DateTime.UtcNow.AddSeconds(60)
                );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            user.TokenExpiryDate = DateTime.UtcNow.AddSeconds(20);

            return token;
        }
        public string ValidateToken(User user)
        {
            if (user.Token == null || user.TokenExpiryDate < DateTime.UtcNow)
            {
                return null;
            }
            user.TokenExpiryDate = DateTime.UtcNow.AddSeconds(20);
            return user.Token;
        }
    }
}
