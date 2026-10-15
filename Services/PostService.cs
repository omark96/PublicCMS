using Microsoft.EntityFrameworkCore;
using PublicCMS.Data;
using PublicCMS.Models;

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
        context.Posts.Add(post);
        await context.SaveChangesAsync();
    }

    public async Task<Post?> GetPostAsync(int id)
    {
        return await context.Posts
            .Include(p => p.Elements)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task UpdatePostAsync(Post post)
    {
        context.Posts.Update(post);
        await context.SaveChangesAsync();
    }
}
