namespace InstagramProjectBack.Models.Dto
{
    public class PostResponseDto<T>
    {
        public bool Success { get; set; } = true;
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;

    }
}