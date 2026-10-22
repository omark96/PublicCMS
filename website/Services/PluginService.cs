using PublicCMS.Models.PageElements;
using PublicCMS.Models.Plugins;
using PublicCMS.Repositories;
using Scriban;
using Scriban.Runtime;

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

        var jsonElement = await pluginRepository.GetPluginDataAsync(dataUrl);
        if (jsonElement is null)
        {
            return """<p class="text-danger"> COULDN'T GET PLUGIN DATA </p>""";
        }

        model["data"] = ScriptObject.From(jsonElement);

        var context = new TemplateContext { LoopLimit = 1000 };
        context.PushGlobal(model);

        return Template.Parse(plugin.PluginDefinition.Template).Render(context);
    }
}
