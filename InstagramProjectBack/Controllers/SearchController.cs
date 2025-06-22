using Microsoft.AspNetCore.Mvc;

namespace InstagramProjectBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string searchQuery)
        {
            try
            {
                if (string.IsNullOrEmpty(searchQuery))
                {
                    return BadRequest("Query is required");
                }

                // Your search logic here

                return Ok(); // Make sure this exists!
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}