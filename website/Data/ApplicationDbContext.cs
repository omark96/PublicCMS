using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PublicCMS.Models;
using PublicCMS.Models.PageElements;
using PublicCMS.Models.Plugins;
using System.Text.Json;

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

        builder.Entity<Post>()
            .HasIndex(p => p.Slug)
            .IsUnique();

        builder.Entity<PostElement>()
            .HasDiscriminator<string>("ElementType")
            .HasValue<ParagraphElement>("Paragraph")
            .HasValue<ImageElement>("Image")
            .HasValue<PluginElement>("Plugin");

        builder.Entity<PluginElement>()
            .Property(p => p.Options)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null) ?? new())
            .Metadata.SetValueComparer(new ValueComparer<Dictionary<string, string>>(
                (a, b) => a!.Count == b!.Count && !a.Except(b).Any(),
                d => 0,
                d => new Dictionary<string, string>(d)));

        builder.Entity<PluginElement>()
            .Navigation(p => p.PluginDefinition).AutoInclude();

        builder.Entity<PluginDefinition>()
            .Navigation(d => d.OptionSources).AutoInclude();
    }
}
