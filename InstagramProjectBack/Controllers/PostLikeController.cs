using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InstagramProjectBack.Repositories;
using InstagramProjectBack.Services;
using InstagramProjectBack.Data;
using InstagramProjectBack.Models;
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

        [Authorize]
        [HttpGet("get Postlikes")]
        public IActionResult GetLikes()
        {
            try
            {
                BaseResponseDto<List<PostLike>> result = _postLikeRepository.GetAllPostLikes();
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
        [HttpPost("create Postlike")]
        public IActionResult CreatePostLike([FromBody] PostLike plike)
        {
            try
            {
                int UserId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                plike.UserId = UserId;
                BaseResponseDto<PostLike> result = _postLikeRepository.CreatePostLike(plike);

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
        [Authorize]
        [HttpDelete("remove postlike")]
        public IActionResult Delete([FromBody] PostLike exsLike )
        {
            try
            {
                int UserId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                exsLike.UserId = UserId;
                BaseResponseDto<PostLike> result = _postLikeRepository.DeletePostLike(exsLike);
                if (result.Success == false)
                {
                    return BadRequest(new { Message = result.Message });
                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
                
        }
    }
}
