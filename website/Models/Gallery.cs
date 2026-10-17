namespace PublicCMS.Models;

public class Gallery
{
    public Guid Guid { get; set; }
    public string Name { get; set; } = "";
    public List<GalleryImage> Images { get; set; } = [];
}
