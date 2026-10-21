using Microsoft.EntityFrameworkCore;
using PublicCMS.Data;
using PublicCMS.Models.PageElements;
using PublicCMS.Models.Plugins;
using Scriban;
using Scriban.Runtime;
using System.Text.Json;

namespace PublicCMS.Services;

public class PluginService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

    public PluginService(IHttpClientFactory httpClientFactory, IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        this.httpClientFactory = httpClientFactory;
        this.contextFactory = contextFactory;
    }
    public async Task<List<PluginOptionDto>> GetOptionsAsync(OptionSource source)
    {
        var client = httpClientFactory.CreateClient();

        var items = await client.GetFromJsonAsync<List<JsonElement>>(source.EndPoint) ?? [];

        List<PluginOptionDto> optionsList = [];

        foreach (var item in items)
        {
            var option = new PluginOptionDto()
            {
                DisplayName = item.TryGetProperty(source.DisplayNameField, out var name) ? name.ToString() : "",
                Value = item.TryGetProperty(source.ValueField, out var value) ? value.ToString() : ""
            };
            optionsList.Add(option);
        }

        return optionsList;
    }

    public async Task<PluginDefinition> CreatePluginDefinitionAsync(string pluginName)
    {
        var context = await contextFactory.CreateDbContextAsync();
        PluginDefinition newPlugin = new()
        {
            Name = pluginName
        };

        context.Add(newPlugin);

        await context.SaveChangesAsync();

        return newPlugin;
    }

    public async Task RemovePluginDefinitionAsync(PluginDefinition plugin)
    {
        var context = await contextFactory.CreateDbContextAsync();
        await context.PluginDefinitions
            .Where(p => p.Id == plugin.Id)
            .ExecuteDeleteAsync();
    }

    public async Task<List<PluginDefinition>> GetPluginDefinitionsAsync()
    {
        var context = await contextFactory.CreateDbContextAsync();
        return await context.PluginDefinitions
            .Include(p => p.OptionSources)
            .ToListAsync();
    }

    public async Task SavePluginDefinitionAsync(PluginDefinition plugin)
    {
        var context = await contextFactory.CreateDbContextAsync();
        context.Update(plugin);
        await context.SaveChangesAsync();
    }

    internal async Task<string> RenderPlugin(PluginElement plugin)
    {
        var client = httpClientFactory.CreateClient();

        var model = new ScriptObject();

        var options = new ScriptObject();

        foreach (var source in plugin.PluginDefinition.OptionSources)
        {
            options[source.Name] = plugin.Options.GetValueOrDefault(source.ValueField, "");
        }

        model["options"] = options;

        var dataUrl = Template.Parse(plugin.PluginDefinition.DataEndpoint).Render(model);
        using var response = await client.GetAsync(dataUrl);
        if (!response.IsSuccessStatusCode)
        {
            return """<p class="text-danger"> COULDN'T GET PLUGIN DATA </p>""";
        }
        var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        model["data"] = ToScriban(doc.RootElement);

        var context = new TemplateContext { LoopLimit = 1000 };
        context.PushGlobal(model);

        string html = Template.Parse(plugin.PluginDefinition.Template).Render(context);

        return html;
    }

    internal static object? ToScriban(JsonElement e)
    {
        switch (e.ValueKind)
        {
            case JsonValueKind.Object:
                var obj = new ScriptObject();
                foreach (var p in e.EnumerateObject())
                {
                    obj[p.Name] = ToScriban(p.Value);
                }
                return obj;
            case JsonValueKind.Array:
                var arr = new ScriptArray();
                foreach (var item in e.EnumerateArray())
                {
                    arr.Add(ToScriban(item));
                }
                return arr;
            case JsonValueKind.String:
                return e.GetString();
            case JsonValueKind.Number:
                if (e.TryGetInt64(out var val))
                {
                    return val;
                }
                return e.GetDouble();
            case JsonValueKind.True:
                return true;
            case JsonValueKind.False:
                return false;
            default: return null;
        }
    }
}
