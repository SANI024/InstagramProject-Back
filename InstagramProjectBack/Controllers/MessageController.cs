using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
using InstagramProjectBack.Services;
using InstagramProjectBack.SignalR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
namespace InstagramProjectBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;
        private readonly TokenService _tokenService;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageController(MessageService messageService, IHubContext<ChatHub> hubContext, TokenService tokenService)
        {
            _messageService = messageService;
            _hubContext = hubContext;
            _tokenService = tokenService;
        }

        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto dto)
        {
            try
            {
                if (!int.TryParse(dto.SenderId, out int senderIntId) || !int.TryParse(dto.ReceiverId, out int receiverIntId))
                {
                    return BadRequest(new { Message = "Invalid sender or receiver ID." });
                }

                var result = await _messageService.ProcessMessageAsync(senderIntId, receiverIntId, dto.Message);

                if (!result.Success)
                {
                    if (result.Message.Contains("not found") || result.Message.Contains("doesn't exist", StringComparison.OrdinalIgnoreCase))
                        return NotFound(new { result.Message });

                    return BadRequest(new { result.Message });
                }

                return Ok(new
                {
                    Message = "Message sent successfully.",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An internal error occurred: {ex.Message}" });
            }
        }

        [HttpPost("getMessages")]
        public async Task<IActionResult> getMessages(getMessagesRequestDto dto)
        {
            try
            {
                int loggedInUserId = _tokenService.GetUserIdFromHttpContext(HttpContext);
                var result = await _messageService.getMessagesAsync(loggedInUserId, dto.userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An internal error occurred: {ex.Message}" });
            }
        }

    }
}