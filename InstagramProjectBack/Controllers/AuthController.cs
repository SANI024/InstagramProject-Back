using InstagramProjectBack.Models.Dto;
using InstagramProjectBack.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InstagramProjectBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            try
            {
                var response = await _authService.Register(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto dto)
        {
            try
            {
                var response = _authService.Login(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("send-verification")]
        public async Task<IActionResult> SendVerification([FromQuery] string email)
        {
            try
            {
                var response = await _authService.ResendVerificationTokenAsync(email);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpGet("verify")]
        public IActionResult VerifyEmail([FromQuery] string token)
        {
            try
            {
                bool UserVerification = _authService.VerifyUser(token);
                if (UserVerification == false)
                {
                    return BadRequest(new { Message = "user verification failed." });
                }
                return Ok(new {Message = "User Verified Successfully!"});

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }            
        }

    }
}
