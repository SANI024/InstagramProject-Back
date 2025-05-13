namespace InstagramProjectBack.Models.Dto
{
    public class UserRegisterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? ProfileImage { get; set; } = string.Empty;
    }
}
