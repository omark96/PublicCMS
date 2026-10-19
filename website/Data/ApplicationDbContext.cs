using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PublicCMS.Models;
using PublicCMS.Models.PageElements;
using PublicCMS.Models.Plugins;

namespace PublicCMS.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostElement> PageElements { get; set; }
    public DbSet<NavMenuItem> NavMenuItems { get; set; }
    public DbSet<Theme> Themes { get; set; }
    public DbSet<PluginDefinition> PluginDefinitions { get; set; }
    public DbSet<OptionSource> OptionSources { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("website");

        builder.Entity<PostElement>()
            .HasDiscriminator<string>("ElementType")
            .HasValue<ParagraphElement>("Paragraph")
            .HasValue<ImageElement>("Image");
    }
}
