using Microsoft.EntityFrameworkCore;
using PublicCMS.Data;
using PublicCMS.Models;

namespace PublicCMS.Services;

public class SiteSectionService
{
    private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

    public SiteSectionService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public async Task<List<SiteSection>> GetSiteSectionsAsync()
    {
        var context = await contextFactory.CreateDbContextAsync();
        return await context.SiteSections.ToListAsync();
    }

    public async Task<SiteSection?> GetSiteSectionByNameAsync(string name)
    {
        var context = await contextFactory.CreateDbContextAsync();
        return await context.SiteSections.FirstOrDefaultAsync(s => s.Name == name);
    }

    public async Task<SiteSection> CreateSiteSectionAsync()
    {
        var context = await contextFactory.CreateDbContextAsync();
        SiteSection section = new() { Name = Guid.NewGuid().ToString() };
        context.Add(section);
        await context.SaveChangesAsync();
        return section;
    }

    public async Task SaveSiteSectionAsync(SiteSection section)
    {
        var context = await contextFactory.CreateDbContextAsync();
        context.Update(section);
        await context.SaveChangesAsync();
    }

    public async Task RemoveSiteSectionAsync(SiteSection section)
    {
        var context = await contextFactory.CreateDbContextAsync();
        context.Remove(section);
        await context.SaveChangesAsync();
    }

    public async Task<bool> SiteSectionHasValidName(SiteSection section)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var existing = await context.SiteSections.AnyAsync(s => s.Name == section.Name && s.Id != section.Id);
        return !existing;
    }
}
