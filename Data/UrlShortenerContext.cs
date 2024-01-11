using Microsoft.EntityFrameworkCore;
using ShortURL.Models;

namespace ShortURL.Data
{
    public class UrlShortenerContext : DbContext
    {
        public UrlShortenerContext(DbContextOptions<UrlShortenerContext> options) : base(options) {    }
        public DbSet<User> Users { get; set; }
        public DbSet<RegLog> RegLogs { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<UrlLog> UrlLogs { get; set; }
       

    }
}
