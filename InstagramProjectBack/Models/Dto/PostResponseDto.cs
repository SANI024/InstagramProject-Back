namespace InstagramProjectBack.Models.Dto
{
    public class PostResponseDto
    {
        Post? post { get; set; }
        string Message { get; set; } = string.Empty;
    }
}