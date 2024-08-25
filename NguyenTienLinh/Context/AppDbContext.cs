using Microsoft.EntityFrameworkCore;
using NguyenTienLinh.Models;

namespace NguyenTienLinh.Context
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("Conn");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public DbSet<Videos> Videos { get; set; }
        public DbSet<Categories> Categories
        { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<BackGround> BackGround { get; set; }

        public DbSet<About> About { get; set; }
    }

}
