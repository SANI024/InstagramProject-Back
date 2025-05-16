namespace InstagramProjectBack.Models.Dto
{
    public class CreatePostDto
    {
        public int UserId { get; set; }
        public string? VideoUrl { get; set; }
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}