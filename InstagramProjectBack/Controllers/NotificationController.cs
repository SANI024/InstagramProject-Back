using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InstagramProjectBack.Models;
using InstagramProjectBack.Models.Dto;
using InstagramProjectBack.Services;
using InstagramProjectBack.Repositories;

namespace InstagramProjectBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationRepository _notificationrepo;

        public NotificationController(NotificationRepository notificationrepo)
        {
            _notificationrepo = notificationrepo;
        }

        // [HttpGet("{receiverId}")]
        // public IActionResult GetUserNotifications(int receiverId)
        // {
        //     try
        //     {
        //         var response = _notificationrepo.GetUserNotifications(receiverId);

        //         if (!response.Success)
        //             return NotFound(response);

        //         return Ok(response);

        //     }catch (Exception ex)
        //     {
        //         return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
        //     }
            
        // }

    }
}
