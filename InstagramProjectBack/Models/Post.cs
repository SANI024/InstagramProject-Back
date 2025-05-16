namespace InstagramProjectBack.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string VideoUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public User? User { get; set; }
        public ICollection<PostLike>? Likes { get; set; }
        public ICollection<PostComment>? Comments { get; set; }

    }
}