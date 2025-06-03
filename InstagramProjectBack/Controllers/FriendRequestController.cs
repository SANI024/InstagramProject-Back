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
                int senderId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                var result = _friendRequestRepository.SendFriendRequest(senderId, dto.Reciver_Id);

                if (!result.Success)
                {
                    if (result.Message.Contains("already"))
                        return Conflict(new { result.Message }); // 409 
                    return BadRequest(new { result.Message }); // 400 
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet("get")]
        public IActionResult GetFriendRequests()
        {
            try
            {
                int receiverId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                var result = _friendRequestRepository.GetFriendRequestsByReciverId(receiverId);

                if (!result.Success)
                    return NotFound(new { result.Message }); // 404 

                return Ok(new { FriendRequests = result.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPatch("accept")]
        public IActionResult AcceptFriendRequest([FromBody] AcceptFriendRequestDto dto)
        {
            try
            {
                int receiverId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                int senderId = dto.Sender_Id;

                var result = _friendRequestRepository.AcceptFriendRequest(senderId, receiverId);

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
        public IActionResult RejectFriendRequest([FromBody] RejectFriendRequestDto dto)
        {
            try
            {
                int receiverId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                int senderId = dto.Sender_Id;

                var result = _friendRequestRepository.RejectFriendRequest(senderId, receiverId);

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
    }
}
