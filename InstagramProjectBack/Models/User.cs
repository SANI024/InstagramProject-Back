namespace InstagramProjectBack.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string PasswordHash { get; set; } = string.Empty;
        public string? Token { get; set; }
        public DateTime TokenExpiryDate { get; set; }
    }
}
