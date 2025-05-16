namespace InstagramProjectBack.Models.Dto
{
    public class UpdatePostDto
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? Description { get; set; }
    }
}