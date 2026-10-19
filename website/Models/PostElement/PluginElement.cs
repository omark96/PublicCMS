namespace PublicCMS.Models.PageElements;

using PublicCMS.Models.Plugins;


public class PluginElement : PostElement
{
    public int PluginDefinitionId { get; set; }
    public Dictionary<string, string> Options { get; set; } = [];
    public PluginDefinition? PluginDefinition { get; set; }
}
