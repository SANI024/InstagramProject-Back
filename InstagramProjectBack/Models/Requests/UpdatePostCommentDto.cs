namespace InstagramProjectBack.Models.Dto
{
    public class UpdatePostCommentDto
    {
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public string Text { get; set; } = string.Empty;

    }
}