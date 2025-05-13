namespace InstagramProjectBack.Models.Dto
{
    public class AuthResponceDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
