using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ImageCollector.Domain.Entities;
 
namespace ImageCollector.Infrastructure.Data
{
    public class ImageCollectorContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Image> Images { get; set; }
        public DbSet<Location> Locations { get; set; }

        public ImageCollectorContext(DbContextOptions<ImageCollectorContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Image>().HasIndex(i => i.ImageUrl).IsUnique();
        }
    }
}
