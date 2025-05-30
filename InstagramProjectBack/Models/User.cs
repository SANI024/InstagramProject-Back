﻿namespace InstagramProjectBack.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PofileImage { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsVerified{ get; set; }
        public DateTime TokenExpiryDate { get; set; }
    }
}
