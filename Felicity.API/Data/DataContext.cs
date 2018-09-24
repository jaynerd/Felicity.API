using Microsoft.EntityFrameworkCore;
using Felicity.API.Models;

namespace Felicity.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamUser> TeamUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Team>().ToTable("Teams");

            modelBuilder.Entity<TeamUser>()
                .ToTable("TeamUsers")
                .HasKey(tu => new { tu.TeamId, tu.UserId });
        }
    }
}
