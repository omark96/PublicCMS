using Microsoft.EntityFrameworkCore;
using PublicCMS.Data;
using PublicCMS.Models;

namespace PublicCMS.Repositories;

public class SiteSectionRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

    public SiteSectionRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public async Task<List<SiteSection>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.SiteSections.ToListAsync();
    }

    public async Task<SiteSection?> GetByNameAsync(string name)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.SiteSections.FirstOrDefaultAsync(s => s.Name == name);
    }

    public async Task<bool> NameExistsAsync(string name, int excludeSectionId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.SiteSections.AnyAsync(s => s.Name == name && s.Id != excludeSectionId);
    }

    public async Task AddAsync(SiteSection section)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.Add(section);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SiteSection section)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.Update(section);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(SiteSection section)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.Remove(section);
        await context.SaveChangesAsync();
    }
}
