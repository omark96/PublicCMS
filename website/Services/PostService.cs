using Microsoft.EntityFrameworkCore;
using PublicCMS.Data;
using PublicCMS.Models;
using PublicCMS.Models.PageElements;

namespace PublicCMS.Services;

public class PostService
{
    private readonly ApplicationDbContext context;

    public PostService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Post>> GetAllPostsAsync()
    {
        return await context.Posts.ToListAsync();
    }

    public async Task CreatePostAsync(Post post)
    {
        post.CreatedAt = DateTime.UtcNow;
        post.Summary = GenerateSummary(post);
        context.Posts.Add(post);
        await context.SaveChangesAsync();
    }

    public async Task<Post?> GetPostAsync(int id)
    {
        return await context.Posts
            .Include(p => p.Elements)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task UpdatePostAsync(Post post)
    {
        post.Summary = GenerateSummary(post);
        context.Posts.Update(post);
        await context.SaveChangesAsync();
    }

    public async Task IncrementVisitorCountAsync(int postId)
    {
        await context.Posts
            .Where(p => p.Id == postId)
            .ExecuteUpdateAsync(
            s => s.SetProperty(p => p.VisitorCount, p => p.VisitorCount + 1));
    }

    private string GenerateSummary(Post post)
    {
        string summary = "";
        foreach (var element in post.Elements)
        {
            if (element is ParagraphElement paragraph)
            {
                bool inTag = false;
                foreach (var c in paragraph.Content)
                {
                    switch (c)
                    {
                        case '<':
                            inTag = true;
                            break;
                        case '>':
                            inTag = false;
                            break;
                        default:
                            if (!inTag)
                            {
                                summary += c;
                            }
                            break;
                    }
                }
            }
            summary += " ";
        }
        if (summary.Length > 50)
        {
            summary = summary.Substring(0, 50);
            summary += "...";
        }
        return summary;
    }
}
