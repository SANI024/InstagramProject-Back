using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
using InstagramProjectBack.Repositories;
using InstagramProjectBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using InstagramProjectBack.Models.Dto;
using System.Security.Claims;
using InstagramProjectBack.Data;

namespace InstagramProjectBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;
        private readonly TokenService _tokenService;
        private readonly EmailService _emailService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthController(IAuthService authService, TokenService tokenService, EmailService emailService, IPasswordHasher<User> passwordHasher)
        {
            _authService = authService;
            _tokenService = tokenService;
            _emailService = emailService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            try
            {
                var response = await _authService.Register(dto);

                if (!response.Success)
                {
                    if (response.Message.Contains("already"))
                        return Conflict(new { response.Message });
                    return BadRequest(new { response.Message });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Server error", Details = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            try
            {
                var response = await _authService.Login(dto);

                if (!response.Success)
                    return Unauthorized(new { response.Message });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Server error", Details = ex.Message });
            }
        }

        [HttpPost("send-verification")]
        public async Task<IActionResult> SendVerification([FromQuery] string email)
        {
            try
            {
                var response = await _authService.ResendVerificationTokenAsync(email);

                if (!response.Success)
                {
                    if (response.Message.Contains("not found"))
                        return NotFound(new { response.Message });
                    return BadRequest(new { response.Message });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Server error", Details = ex.Message });
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyRequestDto dto)
        {
            try
            {
                bool isVerified = await _authService.VerifyUserAsync(dto.token);

                if (!isVerified)
                    return BadRequest(new { Message = "User verification failed." });

                return Ok(new { Message = "User verified successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Server error", Details = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                int userId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                var result = await _authService.GetUserAsync(userId);
                if (result.Success == false)
                {
                    return BadRequest(new { Message = "invalid user id." });
                }

                return Ok(new { User = result.Data });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Message = "Server error", Details = ex.Message });
            }


        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPassowrdRequestDto dto)
        {
            try
            {
                BaseResponseDto<User> result = await _authService.GetUserByEmailAsync(dto.Email);
                if (result == null)
                {
                    return BadRequest(new { message = $"user with email: {dto.Email} was not found." });
                }
                if (string.IsNullOrEmpty(result.Data.Email))
                {
                    return StatusCode(500, new { message = "User email is missing." });
                }
                if (result.Data.PasswordResetTokenCreatedAt.HasValue &&
                   DateTime.UtcNow - result.Data.PasswordResetTokenCreatedAt.Value < TimeSpan.FromMinutes(60))
                {
                    return BadRequest(new { message = "Please wait before requesting another reset email." });
                }


                string token = _tokenService.GeneratePasswordResetToken(result.Data.Id.ToString(), result.Data.Email);
                var response = await _authService.UpdateUserAsync(result.Data.Id, passwordResetToken: token);
                await _emailService.SendPasswordResetEmail(result.Data.Email, token);
                return Ok(new { message = "Reset password link sent to your email." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Message = "Server error", Details = ex.Message });
            }
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto dto)
        {
            try
            {
                Console.WriteLine("Getting principal from token");
                var principal = _tokenService.GetPrincipalFromToken(dto.Token);
                Console.WriteLine(principal == null ? "principal is null" : "principal found");
                if (principal == null)
                    return BadRequest(new { message = "Invalid token." });

                string userIdStr = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine($"userIdStr: {userIdStr}");
                if (string.IsNullOrEmpty(userIdStr))
                    return BadRequest(new { message = "Invalid token data." });

                if (!int.TryParse(userIdStr, out int userId))
                    return BadRequest(new { message = "Invalid user ID in token." });

                var userResponse = await _authService.GetUserAsync(userId);
                if (userResponse?.Data == null)
                    return BadRequest(new { message = "User not found." });

                var user = userResponse.Data;

                if (user.PasswordResetToken != dto.Token)
                    return BadRequest(new { message = "Invalid or expired token." });

                if (!user.PasswordResetTokenCreatedAt.HasValue ||
                    DateTime.UtcNow - user.PasswordResetTokenCreatedAt.Value > TimeSpan.FromHours(1))
                    return BadRequest(new { message = "Token expired." });

                var updateResponse = await _authService.ResetPasswordAsync(userId, dto.NewPassword);
                if (!updateResponse.Success)
                    return BadRequest(new { message = updateResponse.Message });

                return Ok(new { message = "Password successfully reset." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Server error", Details = ex.Message });
            }
        }


        [HttpGet("Users/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var result = await _authService.GetUserAsync(id);
                if (result == null)
                    return NotFound(new { Message = "User not found" });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Server error", Details = ex.Message });
            }
        }


        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Alive");
        }
    }
}
