namespace PublicCMS_API.Models;

public class GalleryImage
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public string Name { get; set; } = "";
    public string Path { get; set; } = "";
    public int GalleryId { get; set; }
    public Gallery Gallery { get; set; }
}

