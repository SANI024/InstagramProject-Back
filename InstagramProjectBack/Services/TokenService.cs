using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using InstagramProjectBack.Models;

using Microsoft.IdentityModel.Tokens;


namespace InstagramProjectBack.Services
{

    public class TokenService
    {

        string JwtIssuerProd = Environment.GetEnvironmentVariable("Jwt_Issuer_Production");
        string JwtAudienceProd = Environment.GetEnvironmentVariable("Jwt_Audience_Production");
        string issuer = Environment.GetEnvironmentVariable("Jwt_Issuer_Production");
        string jwtSecret = Environment.GetEnvironmentVariable("Jwt_Secret");
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
             new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Jwt_Secret")));

            logger.LogInformation($"key: {key}");

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            logger.LogInformation($"creds: {creds}");

            var tokenDescriptor = new JwtSecurityToken(
                issuer: JwtIssuerProd,
                audience: JwtAudienceProd,
                claims: claims,
                signingCredentials: creds,
                expires: DateTime.UtcNow.AddSeconds(60)
                );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            user.TokenExpiryDate = DateTime.UtcNow.AddSeconds(20);

            return token;
        }

        public string GeneratePasswordResetToken(string userId, string email)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, userId)
          };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Jwt_Secret")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: JwtAudienceProd,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int GetUserIdFromHttpContext(HttpContext httpContext)
        {
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token");

            return int.Parse(userIdClaim.Value);
        }

        public ClaimsPrincipal? GetPrincipalFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Jwt_Secret"));

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = JwtAudienceProd,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                logger.LogWarning($"Loaded issuer: '{issuer}'");
                return principal;
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Token validation failed: {ex.Message}");
                return null;
            }
        }

    }
}
