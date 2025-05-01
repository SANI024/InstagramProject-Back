using Microsoft.EntityFrameworkCore;
using System;

namespace InstagramProjectBack.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
