public class PostCommentDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public UserDto? User { get; set; }
}
