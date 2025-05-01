using GeminiApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeminiApp.Data
{
    public class GeminiDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ContentRequest> ContentRequests { get; set; }

        public GeminiDbContext(DbContextOptions<GeminiDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ContentRequest>()
                .HasOne(cr => cr.User)
                .WithMany()
                .HasForeignKey(cr => cr.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
