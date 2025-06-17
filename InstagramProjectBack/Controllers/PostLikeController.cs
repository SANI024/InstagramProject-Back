using Microsoft.AspNetCore.Mvc;
using InstagramProjectBack.Services;
using Microsoft.AspNetCore.Authorization;
using InstagramProjectBack.Models.Interfaces;
using InstagramProjectBack.Models.Dto;


namespace InstagramProjectBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostLikeController : ControllerBase
    {
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly TokenService _tokenService;

        public PostLikeController(IPostLikeRepository postRepository, TokenService tokenService)
        {
            _postLikeRepository = postRepository;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpGet("get all post likes")]
        public async Task<IActionResult> GetAllPostLikes()
        {
            try
            {
                var result = await _postLikeRepository.GetAllPostLikesAsync();

                if (!result.Success)
                    return NotFound(new { result.Message });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpPost("like post")]
        public async Task<IActionResult> CreatePostLike([FromBody] PostLikeRequestDto dto)
        {
            try
            {
                int userId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                dto.UserId = userId;

                var result = await _postLikeRepository.CreatePostLikeAsync(dto);

                if (!result.Success)
                    return BadRequest(new { result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpDelete("unlike post")]
        public async Task<IActionResult> DeletePostLike([FromBody] PostDislikeRequestDto dto)
        {
            try
            {
                int userId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                dto.UserId = userId;

                var result = await _postLikeRepository.DeletePostLikeAsync(dto);

                if (!result.Success)
                    return BadRequest(new { result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
