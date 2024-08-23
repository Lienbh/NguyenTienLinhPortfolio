using Microsoft.EntityFrameworkCore;
using NguyenTienLinh.Models;

namespace NguyenTienLinh.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=LAPTOP-A9Q63JRK\\SQLEXPRESS;Database=ProfileDatabase;Trusted_Connection=True;TrustServerCertificate=True;Connection Timeout=120;");
        }

        public DbSet<Videos> Videos { get; set; }
        public DbSet<Categories> Categories
        { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<BackGround> BackGround { get; set; }

        public DbSet<About> About { get; set; }
    }

}
