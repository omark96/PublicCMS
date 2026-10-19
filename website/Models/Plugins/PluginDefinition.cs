namespace PublicCMS.Models.Plugins;

public class PluginDefinition
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string DataEndpoint { get; set; } = "";
    public List<OptionSource> OptionSources { get; set; } = [];
    public string Template { get; set; } = "";
}
