using PublicCMS.Models;
using PublicCMS.Repositories;

namespace PublicCMS.Services;

public class ThemeService
{
    private readonly ThemeRepository themeRepository;

    public ThemeService(ThemeRepository themeRepository)
    {
        this.themeRepository = themeRepository;
    }

    public async Task<List<Theme>> GetThemesAsync()
    {
        return await themeRepository.GetAllAsync();
    }

    public async Task<Theme> CreateThemeAsync(string name)
    {
        Theme newTheme = Theme.DefaultTheme();
        newTheme.Name = name;

        await themeRepository.AddAsync(newTheme);

        return newTheme;
    }

    public async Task<Theme?> GetThemeAsync(int id)
    {
        return await themeRepository.GetByIdAsync(id);
    }

    public async Task SaveThemeAsync(Theme theme)
    {
        theme.GenerateCss();
        await themeRepository.UpdateAsync(theme);
    }

    public async Task SetSelectedThemeAsync(Theme theme)
    {
        await themeRepository.SetSelectedAsync(theme.Id);
        theme.IsSelected = true;
    }

    public async Task<Theme?> GetSelectedThemeAsync(Theme theme)
    {
        return await themeRepository.GetSelectedAsync();
    }

    public async Task RemoveThemeAsync(Theme theme)
    {
        await themeRepository.RemoveAsync(theme);
    }
}
