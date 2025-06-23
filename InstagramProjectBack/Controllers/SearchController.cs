using InstagramProjectBack.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace InstagramProjectBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchRepository _searchRepository;
        public SearchController(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }
        [HttpGet("users")]
        public async Task<IActionResult> SearchUsers([FromQuery] string searchQuery)
        {
            try
            {
                if (string.IsNullOrEmpty(searchQuery))
                {
                    return BadRequest("Query is required");
                }
                var result = await _searchRepository.SearchUsersAsync(searchQuery);
                if (!result.Success)
                {
                    return BadRequest(new { Message = $"{result.Message}" });
                }


                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet("posts")]
        public async Task<IActionResult> SearchPosts([FromQuery] string searchQuery)
        {
            try
            {
                if (string.IsNullOrEmpty(searchQuery))
                {
                    return BadRequest("Query is required");
                }
                var result = await _searchRepository.SearchPostsAsync(searchQuery);
                if (!result.Success)
                {
                    return BadRequest(new { Message = $"{result.Message}" });
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