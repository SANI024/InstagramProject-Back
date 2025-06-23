using InstagramProjectBack.Models.Dto;
using InstagramProjectBack.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace InstagramProjectBack.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendRequestController : ControllerBase
    {
        private readonly FriendRequestService _friendRequestService;
        private readonly TokenService _tokenService;

        public FriendRequestController(FriendRequestService friendRequestService, TokenService tokenService)
        {
            _friendRequestService = friendRequestService;
            _tokenService = tokenService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendFriendRequest([FromBody] SendFriendRequestDto dto)
        {
            try
            {
                int senderId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                var result = await _friendRequestService.SendFriendRequestServiceAsync(senderId, dto.Reciver_Id);

                if (!result.Success)
                {
                    if (result.Message.Contains("already"))
                        return Conflict(new { result.Message });
                    return BadRequest(new { result.Message });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetFriendRequests()
        {
            try
            {
                int receiverId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                var result = await _friendRequestService.GetFriendRequestsServiceAsync(receiverId);
                if (!result.Success)
                    return NotFound(new { result.Message });

                return Ok(new { FriendRequests = result.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPatch("accept")]
        public async Task<IActionResult> AcceptFriendRequest([FromBody] AcceptFriendRequestDto dto)
        {
            try
            {
                int receiverId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                int senderId = dto.Sender_Id;

                var result = await _friendRequestService.AcceptFriendRequestServiceAsync(senderId, receiverId);

                if (!result.Success)
                {
                    if (result.Message.Contains("not found"))
                        return NotFound(new { result.Message });
                    return BadRequest(new { result.Message });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPatch("reject")]
        public async Task<IActionResult> RejectFriendRequest([FromBody] RejectFriendRequestDto dto)
        {
            try
            {
                int receiverId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                int senderId = dto.Sender_Id;

                var result = await _friendRequestService.RejectFriendRequestServiceAsync(senderId, receiverId);

                if (!result.Success)
                {
                    if (result.Message.Contains("not found"))
                        return NotFound(new { result.Message });
                    return BadRequest(new { result.Message });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet("GetFriends")]
        public async Task<IActionResult> GetFriends()
        {
            try
            {
                int userId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                var result = await _friendRequestService.getFriendsServiceAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost("isFriend")]
        public async Task<IActionResult> isFriend([FromBody] IsFriendRequestDto dto)
        {
            try
            {
                int checkerId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                var result = await _friendRequestService.isFriendServiceAsync(dto.userId, checkerId);
                return Ok(result.Success);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

    }
}
