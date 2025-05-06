namespace InstagramProjectBack.Models.Dto
{
    public class UserRegisterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string Password { get; set; } = string.Empty;
    }
}
