using Microsoft.EntityFrameworkCore;
using PublicCMS_API.Models;

namespace PublicCMS_API.Data;

public class ApiDbContext(DbContextOptions<ApiDbContext> options) : DbContext(options)
{
    public DbSet<Gallery> Galleries { get; set; }
    public DbSet<GalleryImage> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("api");
    }
}
