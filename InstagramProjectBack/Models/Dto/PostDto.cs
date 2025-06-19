public class PostDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? VideoUrl { get; set; }
    public string? ImageUrl { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public UserDto User { get; set; }

    public List<PostCommentDto>? Comments { get; set; }
    public List<PostLikeDto>? Likes { get; set; }
}