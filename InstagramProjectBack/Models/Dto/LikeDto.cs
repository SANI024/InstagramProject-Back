public class PostLikeDto
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public UserDto? User { get; set; }
}
