namespace InstagramProjectBack.Models.Dto
{
    public class UpdatePostCommentDto
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Text { get; set; } = string.Empty;

    }
}