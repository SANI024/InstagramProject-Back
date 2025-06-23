using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
using InstagramProjectBack.Repositories;
using InstagramProjectBack.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

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

        [HttpPost("create post")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto dto)
        {
            try
            {
                int userId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                dto.UserId = userId;

                var result = await _postRepository.CreatePostAsync(dto);

                if (!result.Success)
                    return BadRequest(new { result.Message });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [AllowAnonymous]
        [HttpGet("get all posts")]
        public async Task<IActionResult> GetPosts()
        {
            try
            {
                var result = await _postRepository.GetPostsAsync();

                if (!result.Success)
                    return NotFound(new { result.Message });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost("GetPost")]
        public async Task<IActionResult> GetPost([FromBody] GetPostRequestDto dto)
        {
            try
            {
                var result = await _postRepository.GetPostAsync(dto.postId);
                if (!result.Success)
                {
                    return BadRequest(new { result.Message });
                }

                return Ok(new { Post = result.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpDelete("delete post")]
        public async Task<IActionResult> RemovePost([FromBody] RemovePostRequestDto dto)
        {
            try
            {
                int userId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                var result = await _postRepository.RemovePostAsync(dto.PostId, userId);

                if (!result.Success)
                    return BadRequest(new { result.Message });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPatch("update post")]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostDto dto)
        {
            try
            {
                int userId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                dto.UserId = userId;

                var result = await _postRepository.UpdatePostAsync(dto);

                if (!result.Success)
                    return BadRequest(new { result.Message });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpGet("GetLikedPosts")]
        public async Task<IActionResult> GetLikedPosts()
        {
            try
            {
                int userId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                var result = await _postRepository.GetLikedPostsAsync(userId);
                if (!result.Success)
                    return BadRequest(new { result.Message });
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }
        [Authorize]
        [HttpGet("GetCreatedPostsByUser")]
        public async Task<IActionResult> GetCreatedPostsByUser()
        {
            try
            {
                int userId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                var result = await _postRepository.GetCreatedPostByUser(userId);
                if (!result.Success)
                    return BadRequest(new { result.Message });
                return Ok(result.Data);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet("GetCreatedPostsByUser/{id}")]
        public async Task<IActionResult> GetCreatedPostsByUser(int id)
        {
            try
            {
                var result = await _postRepository.GetCreatedPostByUser(id);
                if (!result.Success)
                    return NotFound(new { result.Message });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }


    }
}
