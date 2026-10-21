using PublicCMS.Models.PageElements;

namespace PublicCMS.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Slug { get; set; } = "";
    public string Summary { get; set; } = "";
    public bool IsBlogPost { get; set; }
    public int VisitorCount { get; set; }
    public Guid? FeaturedImageGuid { get; set; }
    public List<PostElement> Elements { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
