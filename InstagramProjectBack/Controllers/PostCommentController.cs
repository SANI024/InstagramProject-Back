using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
using InstagramProjectBack.Models.Interfaces;
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

        [Authorize]
        [HttpPost("CreatePostComment")]
        public IActionResult CreatePost([FromBody] CreatePostCommentDto dto)
        {
            try
            {
                int UserId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                dto.UserId = UserId;
                BaseResponseDto<PostComment> result = _postCommentRepository.CreatePostComment(dto);
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
        [HttpGet("get all postComments")]
        public IActionResult GetAllPostComments()
        {
            try
            {
                BaseResponseDto<List<PostComment>> result = _postCommentRepository.GetPostComments();
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
        [HttpDelete("remove post comment")]
        public IActionResult DeletePostaComment( [FromBody]  int PostId)
        {
            try
            {
                int UserId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                BaseResponseDto<PostComment> result = _postCommentRepository.RemovePostComment(PostId, UserId);
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
        [HttpPut("update post comment") ]
        public IActionResult UpdatePostComment([FromBody] UpdatePostCommentDto dto)
        {
            try
            {
                int UserId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                dto.UserId = UserId;
                BaseResponseDto<PostComment> result = _postCommentRepository.UpdatePostComment(dto);
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

    }
}