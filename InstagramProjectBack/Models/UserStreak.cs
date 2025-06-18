namespace InstagramProjectBack.Models
{
    public class UserStreak
    {
        public int Count{ get; set; }
        public DateTime? LastPostDate { get; set; }
        public DateTime? StreakStartedAt { get; set; }
    }
}
