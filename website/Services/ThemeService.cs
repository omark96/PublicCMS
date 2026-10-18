using Microsoft.EntityFrameworkCore;
using PublicCMS.Data;
using PublicCMS.Models;

namespace PublicCMS.Services;

public class ThemeService
{
    private readonly ApplicationDbContext context;

    public ThemeService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Theme>> GetThemesAsync()
    {
        return await context.Themes.ToListAsync();
    }

    public async Task<Theme> CreateThemeAsync(string name)
    {
        Theme newTheme = Theme.DefaultTheme();
        newTheme.Name = name;

        context.Themes.Add(newTheme);

        await context.SaveChangesAsync();

        return newTheme;
    }

    public async Task<Theme?> GetThemeAsync(int id)
    {
        return await context.Themes.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task SaveThemeAsync(Theme theme)
    {
        theme.GenerateCss();
        context.Themes.Update(theme);
        await context.SaveChangesAsync();
    }

    public async Task SetSelectedThemeAsync(Theme theme)
    {
        var current = await context.Themes
            .FirstOrDefaultAsync(t => t.IsSelected);
        if (current is not null)
        {
            current.IsSelected = false;
        }

        theme.IsSelected = true;
        await context.SaveChangesAsync();
    }

    public async Task<Theme?> GetSelectedThemeAsync(Theme theme)
    {
        return await context.Themes.FirstOrDefaultAsync(t => t.IsSelected);
    }

    public async Task RemoveThemeAsync(Theme theme)
    {
        context.Themes.Remove(theme);

        await context.SaveChangesAsync();
    }
}
