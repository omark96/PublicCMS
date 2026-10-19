using System.ComponentModel.DataAnnotations.Schema;

namespace PublicCMS.Models.Plugins;

public class OptionSource
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string EndPoint { get; set; } = "";
    public string ValueField { get; set; } = "";
    public string DisplayNameField { get; set; } = "";

    [NotMapped]
    public List<PluginOptionDto> Options { get; set; } = [];
}