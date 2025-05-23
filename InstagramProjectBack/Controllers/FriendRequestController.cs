using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
using InstagramProjectBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstagramProjectBack.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendRequestController : ControllerBase
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly TokenService _tokenService;

        public FriendRequestController(IFriendRequestRepository friendRequestRepository, TokenService tokenService)
        {
            _friendRequestRepository = friendRequestRepository;
            _tokenService = tokenService;
        }

        [HttpPost("send")]
        public IActionResult SendFriendRequest([FromBody] SendFriendRequestDto dto)
        {
            try
            {
                int sender_id = _tokenService.GetUserIdFromHttpContext(HttpContext);
                BaseResponseDto<Friend_Request> result = _friendRequestRepository.SendFriendRequest(sender_id, dto.Reciver_Id);
                if (!result.Success)
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

        [HttpGet("GetFriendRequests")]
        public IActionResult GetFriendRequests()
        {
            try
            {
                int reciver_id = _tokenService.GetUserIdFromHttpContext(HttpContext);
                BaseResponseDto<List<Friend_Request>> FriendRequests = _friendRequestRepository.GetFriendRequestsByReciverId(reciver_id);
                if (!FriendRequests.Success)
                {
                    return NotFound(new { Message = FriendRequests.Message });
                }
                return Ok(new { FriendRequestsList = FriendRequests.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPatch("AcceptFriendRequest")]
        public IActionResult AcceptFriendRequest([FromBody] AcceptFriendRequestDto dto)
        {
            try
            {
                int reciver_id = _tokenService.GetUserIdFromHttpContext(HttpContext);
                int sender_id = dto.Sender_Id;
                BaseResponseDto<Friend_Request> result = _friendRequestRepository.AcceptFriendRequest(sender_id, reciver_id);
                if (!result.Success)
                {
                    return BadRequest(new { result.Message });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPatch("RejectFriendRequest")]
        public IActionResult RejectFriendRequest([FromBody] RejectFriendRequestDto dto)
        {
            try
            {
                int reciver_id = _tokenService.GetUserIdFromHttpContext(HttpContext);
                int sender_id = dto.Sender_Id;
                BaseResponseDto<Friend_Request> result = _friendRequestRepository.RejectFriendRequest(sender_id, reciver_id);
                if (!result.Success)
                {
                    return BadRequest(new { result.Message });
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
