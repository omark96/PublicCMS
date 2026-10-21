using Microsoft.EntityFrameworkCore;
using PublicCMS.Data;
using PublicCMS.Models;

namespace PublicCMS.Repositories;

public class ThemeRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

    public ThemeRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public async Task<List<Theme>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Themes.ToListAsync();
    }

    public async Task<Theme?> GetByIdAsync(int id)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Themes.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Theme?> GetSelectedAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Themes.FirstOrDefaultAsync(t => t.IsSelected);
    }

    public async Task AddAsync(Theme theme)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.Themes.Add(theme);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Theme theme)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.Themes.Update(theme);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Theme theme)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.Themes.Remove(theme);
        await context.SaveChangesAsync();
    }

    public async Task SetSelectedAsync(int themeId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await context.Themes
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.IsSelected, t => t.Id == themeId));
    }
}
