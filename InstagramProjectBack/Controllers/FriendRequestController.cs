
using InstagramProjectBack.Models;
using InstagramProjectBack.Services;
using Microsoft.AspNetCore.Mvc;
namespace InstagramProjectBack.Controllers
{
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
            int sender_id = _tokenService.GetUserIdFromHttpContext(HttpContext);
            object result = _friendRequestRepository.SendFriendRequest(sender_id, dto.Reciver_Id);
            return Ok(result);
        }
        [HttpGet("getFriendRequests")]
        public IActionResult getFriendRequests()
        {
            int reciver_id = _tokenService.GetUserIdFromHttpContext(HttpContext);
            List<Friend_Request> FriendRequests = _friendRequestRepository.GetFriendRequestsByReciverId(reciver_id);
            return Ok(new { FriendRequestsList = FriendRequests });
        }
    }
}