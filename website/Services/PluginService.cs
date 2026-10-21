using PublicCMS.Models.PageElements;
using PublicCMS.Models.Plugins;
using PublicCMS.Repositories;
using Scriban;
using Scriban.Runtime;
using System.Text.Json;

namespace PublicCMS.Services;

public class PluginService
{
    private readonly PluginRepository pluginRepository;

    public PluginService(PluginRepository pluginRepository)
    {
        this.pluginRepository = pluginRepository;
    }

    public async Task<List<PluginOptionDto>> GetOptionsAsync(OptionSource source)
    {
        var items = await pluginRepository.GetOptionItemsAsync(source.EndPoint);

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
        var newPlugin = new PluginDefinition { Name = pluginName };
        await pluginRepository.AddAsync(newPlugin);
        return newPlugin;
    }

    public async Task RemovePluginDefinitionAsync(PluginDefinition plugin)
    {
        await pluginRepository.DeleteAsync(plugin.Id);
    }

    public async Task<List<PluginDefinition>> GetPluginDefinitionsAsync()
    {
        return await pluginRepository.GetAllAsync();
    }

    public async Task SavePluginDefinitionAsync(PluginDefinition plugin)
    {
        await pluginRepository.UpdateAsync(plugin);
    }

    internal async Task<string> RenderPlugin(PluginElement plugin)
    {
        var model = new ScriptObject();
        var options = new ScriptObject();

        foreach (var source in plugin.PluginDefinition.OptionSources)
        {
            options[source.Name] = plugin.Options.GetValueOrDefault(source.ValueField, "");
        }

        model["options"] = options;

        var dataUrl = Template.Parse(plugin.PluginDefinition.DataEndpoint).Render(model);

        using var doc = await pluginRepository.GetPluginDataAsync(dataUrl);
        if (doc is null)
        {
            return """<p class="text-danger"> COULDN'T GET PLUGIN DATA </p>""";
        }

        model["data"] = ToScriban(doc.RootElement);

        var context = new TemplateContext { LoopLimit = 1000 };
        context.PushGlobal(model);

        return Template.Parse(plugin.PluginDefinition.Template).Render(context);
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
