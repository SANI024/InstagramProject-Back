namespace InstagramProjectBack.Models.Dto
{
    public class CreatePostCommentDto
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Text { get; set; } = string.Empty;

    }
}