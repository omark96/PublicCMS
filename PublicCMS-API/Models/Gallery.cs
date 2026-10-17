namespace PublicCMS_API.Models;

public class Gallery
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public string Name { get; set; } = "";
    public List<GalleryImage> Images { get; set; } = new();
}
