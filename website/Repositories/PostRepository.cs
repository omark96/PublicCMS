using Microsoft.EntityFrameworkCore;
using PublicCMS.Data;
using PublicCMS.Models;

namespace PublicCMS.Repositories;

public class PostRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

    public PostRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public async Task<List<Post>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Posts.ToListAsync();
    }

    public async Task<List<Post>> GetAllBlogPostsAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Posts
            .Where(p => p.IsBlogPost)
            .ToListAsync();
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Posts
            .Include(p => p.Elements.OrderBy(e => e.Order))
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Post?> GetBySlugAsync(string slug)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Posts
            .Include(p => p.Elements.OrderBy(e => e.Order))
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<bool> SlugExistsAsync(string slug, int excludePostId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Posts.AnyAsync(p => p.Slug == slug && p.Id != excludePostId);
    }

    public async Task AddAsync(Post post)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.Posts.Add(post);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Post post)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.Posts.Update(post);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await context.Posts
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task IncrementVisitorCountAsync(int postId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await context.Posts
            .Where(p => p.Id == postId)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.VisitorCount, p => p.VisitorCount + 1));
    }
}
