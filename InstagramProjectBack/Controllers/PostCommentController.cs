using InstagramProjectBack.Models.Dto;
using InstagramProjectBack.Repositories;
using InstagramProjectBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstagramProjectBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostCommentController : ControllerBase
    {
        private readonly IPostCommentRepository _postCommentRepository;
        private readonly TokenService _tokenService;

        public PostCommentController(IPostCommentRepository postCommentRepository, TokenService tokenService)
        {
            _postCommentRepository = postCommentRepository;
            _tokenService = tokenService;
        }

        [HttpPost("create post comments")]
        public async Task<IActionResult> CreatePostComment([FromBody] CreatePostCommentDto dto)
        {
            try
            {
                int userId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                dto.UserId = userId;

                var result = await _postCommentRepository.CreatePostCommentAsync(dto);

                if (!result.Success)
                    return BadRequest(new { result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet("get all post comments")]
        public async Task<IActionResult> GetAllPostComments()
        {
            try
            {
                var result = await _postCommentRepository.GetPostCommentsAsync();

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
        [HttpDelete("delete post comments")]
        public async Task<IActionResult> DeletePostComment([FromBody] int postId)
        {
            try
            {
                int userId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                var result = await _postCommentRepository.RemovePostCommentAsync(userId, postId);

                if (!result.Success)
                    return BadRequest(new { result.Message });

                return Ok(new { result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpPut("update post comments")]
        public async Task<IActionResult> UpdatePostComment([FromBody] UpdatePostCommentDto dto)
        {
            try
            {
                int userId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                dto.UserId = userId;

                var result = await _postCommentRepository.UpdatePostCommentAsync(dto);

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
