using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
using InstagramProjectBack.Repositories;
using InstagramProjectBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstagramProjectBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly TokenService _tokenService;
        public PostController(IPostRepository postRepository, TokenService tokenService)
        {
            _postRepository = postRepository;
            _tokenService = tokenService;
        }

        [Authorize]
        [HttpPost("CreatePost")]
        public IActionResult CreatePost([FromBody] CreatePostDto dto)
        {
            try
            {
                int UserId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                dto.UserId = UserId;
                BaseResponseDto<Post> result = _postRepository.CreatePost(dto);
                if (result.Success == false)
                {
                    return BadRequest(new { Message = result.Message });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet("GetPosts")]
        public IActionResult GetPosts()
        {
            try
            {
                BaseResponseDto<List<Post>> result = _postRepository.GetPosts();
                if (result.Success == false)
                {
                    return BadRequest(new { Message = result.Message });
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }
        [Authorize]
        [HttpDelete("RemovePost")]
        public IActionResult RemovePost([FromBody] int PostId)
        {
            try
            {
                int UserId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                BaseResponseDto<Post> result = _postRepository.RemovePost(PostId, UserId);
                if (result.Success == false)
                {
                    return BadRequest(new { Message = result.Message });
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }
        [Authorize]
        [HttpPatch("UpdatePost")]
        public IActionResult UpdatePost(UpdatePostDto dto)
        {
            try
            {
                int UserId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                dto.UserId = UserId;
                BaseResponseDto<Post> result = _postRepository.UpdatePost(dto);
                if (result.Success == false)
                {
                    return BadRequest(new { Message = result.Message });
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }


    }
}