using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
using InstagramProjectBack.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
namespace InstagramProjectBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageController(MessageService messageService, IHubContext<ChatHub> hubContext)
        {
            _messageService = messageService;
            _hubContext = hubContext;
        }

        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto dto)
        {
            if (!int.TryParse(dto.SenderId, out int senderIntId) || !int.TryParse(dto.ReceiverId, out int receiverIntId))
            {
                return BadRequest("Invalid IDs.");
            }

            var result = await _messageService.ProcessMessageAsync(senderIntId, receiverIntId, dto.Message);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(new { Message = $"{result.Message}", Data = result.Data.Text });
        }
    }

}