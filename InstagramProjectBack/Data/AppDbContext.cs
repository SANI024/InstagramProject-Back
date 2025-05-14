using InstagramProjectBack.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace InstagramProjectBack.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Friend_Request> Friend_Requests { get; set; }
    }
}
