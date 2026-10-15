namespace PublicCMS.Models;

public class GalleryImage
{
    public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    public int GalleryId { get; set; }
    public Gallery Gallery { get; set; }
}
