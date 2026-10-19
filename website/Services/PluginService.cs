using Microsoft.EntityFrameworkCore;
using PublicCMS.Data;
using PublicCMS.Models.Plugins;
using System.Text.Json;

namespace PublicCMS.Services;

public class PluginService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ApplicationDbContext context;

    public PluginService(IHttpClientFactory httpClientFactory, ApplicationDbContext context)
    {
        this.httpClientFactory = httpClientFactory;
        this.context = context;
    }
    public async Task<List<string>> GetOptionsAsync(OptionSource source)
    {
        var client = httpClientFactory.CreateClient();

        var items = await client.GetFromJsonAsync<List<JsonElement>>(source.EndPoint) ?? [];

        return items
            .Select(i => i.TryGetProperty(source.FieldName, out var value) ? value.ToString() : null)
            .OfType<string>()
            .ToList();
    }

    public async Task<PluginDefinition> CreatePluginDefinitionAsync(string pluginName)
    {
        PluginDefinition newPlugin = new()
        {
            Name = pluginName
        };

        context.Add(newPlugin);

        await context.SaveChangesAsync();

        return newPlugin;
    }

    public async Task<List<PluginDefinition>> GetPluginDefinitionsAsync()
    {
        return await context.PluginDefinitions
            .Include(p => p.OptionSources)
            .ToListAsync();
    }

    public async Task SavePluginDefinitionAsync(PluginDefinition plugin)
    {
        context.Update(plugin);
        await context.SaveChangesAsync();
    }
}
