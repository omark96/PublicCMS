using Microsoft.EntityFrameworkCore;
using PublicCMS.Data;
using PublicCMS.Models;
using PublicCMS.Models.PageElements;
using PublicCMS.Utils;
using System.Text;

namespace PublicCMS.Services;

public class PostService
{
    public IDbContextFactory<ApplicationDbContext> ContextFactory { get; }

    public PostService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        ContextFactory = contextFactory;
    }

    public async Task<List<Post>> GetAllPostsAsync()
    {
        await using var context = await ContextFactory.CreateDbContextAsync();
        return await context.Posts.ToListAsync();
    }

    public async Task CreatePostAsync(Post post)
    {
        await using var context = await ContextFactory.CreateDbContextAsync();
        post.CreatedAt = DateTime.UtcNow;
        post.Summary = GenerateSummary(post);
        post.Slug = post.Title.Slugify();
        context.Posts.Add(post);
        await context.SaveChangesAsync();
    }

    public async Task<bool> PostHasValidSlug(Post post)
    {
        await using var context = await ContextFactory.CreateDbContextAsync();
        string slug = post.Title.Slugify();
        var existing = await context.Posts.AnyAsync(p => p.Slug == slug && p.Id != post.Id);
        return !existing;
    }

    public async Task<Post?> GetPostAsync(int id)
    {
        await using var context = await ContextFactory.CreateDbContextAsync();
        return await context.Posts
            .Include(p => p.Elements)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<Post?> GetPostBySlugAsync(string slug)
    {
        await using var context = await ContextFactory.CreateDbContextAsync();
        return await context.Posts
            .Include(p => p.Elements)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task UpdatePostAsync(Post post)
    {
        await using var context = await ContextFactory.CreateDbContextAsync();
        post.Summary = GenerateSummary(post);
        post.Slug = post.Title.Slugify();
        context.Posts.Update(post);
        await context.SaveChangesAsync();
    }

    public async Task IncrementVisitorCountAsync(int postId)
    {
        await using var context = await ContextFactory.CreateDbContextAsync();
        await context.Posts
            .Where(p => p.Id == postId)
            .ExecuteUpdateAsync(
            s => s.SetProperty(p => p.VisitorCount, p => p.VisitorCount + 1));
    }

    private string GenerateSummary(Post post, int summaryLength = 100)
    {
        StringBuilder summary = new();
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
                                summary.Append(c);
                            }
                            break;
                    }
                }
            }
            summary.Append(" ");
        }
        if (summary.Length > summaryLength - 3)
        {
            summary.Length = summaryLength - 3;
            summary.Append("...");
        }
        summaryLength = summaryLength > summary.Length ? summary.Length : summaryLength;
        return summary.ToString(0, summaryLength);
    }
}
