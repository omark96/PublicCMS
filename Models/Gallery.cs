namespace PublicCMS.Models;

public class Gallery
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<GalleryImage> Images { get; set; } = new();
}
