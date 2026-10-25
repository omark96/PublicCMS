using Microsoft.EntityFrameworkCore;
using PublicCMS.Data;
using PublicCMS.Models.Plugins;
using System.Text.Json;

namespace PublicCMS.Repositories;

// The project originally only used Services, but as I read the spec it required it to be split into both services and repositories.
// AI was used to generate the split and no new functionality (except for where it was required based on the split) was added in this commit.
// See the following commit for the split: https://github.com/omark96/PublicCMS/commit/d482e3fbc8f4ad34076ceea2caa3efb6e6eeaecb
public class PluginRepository
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

    public PluginRepository(IHttpClientFactory httpClientFactory, IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        this.httpClientFactory = httpClientFactory;
        this.contextFactory = contextFactory;
    }

    public async Task<List<JsonElement>> GetOptionItemsAsync(string endpoint)
    {
        var client = httpClientFactory.CreateClient();
        return await client.GetFromJsonAsync<List<JsonElement>>(endpoint) ?? [];
    }

    public async Task<JsonElement?> GetPluginDataAsync(string url)
    {
        var client = httpClientFactory.CreateClient();
        using var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }

    public async Task AddAsync(PluginDefinition plugin)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.Add(plugin);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await context.PluginDefinitions
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<List<PluginDefinition>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.PluginDefinitions
            .Include(p => p.OptionSources)
            .ToListAsync();
    }

    public async Task UpdateAsync(PluginDefinition plugin)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.Update(plugin);
        await context.SaveChangesAsync();
    }
}
