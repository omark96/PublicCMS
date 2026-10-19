using System.ComponentModel.DataAnnotations.Schema;

namespace PublicCMS.Models.Plugins;

public class OptionSource
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string EndPoint { get; set; } = "";
    public string FieldName { get; set; } = "";

    [NotMapped]
    public List<string> Options { get; set; } = [];
}