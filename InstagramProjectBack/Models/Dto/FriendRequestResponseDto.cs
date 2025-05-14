using InstagramProjectBack.Models;

public class FriendRequestResponseDto
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public List<Friend_Request>? Friend_Requests { get; set; }
}