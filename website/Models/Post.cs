using PublicCMS.Models.PageElements;

namespace PublicCMS.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public List<PostElement> Elements { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
