using PublicCMS.Models;
using PublicCMS.Models.PageElements;
using PublicCMS.Repositories;
using PublicCMS.Utils;
using System.Text;

namespace PublicCMS.Services;

public class PostService
{
    private readonly PostRepository postRepository;

    public PostService(PostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public async Task<List<Post>> GetAllPostsAsync()
    {
        return await postRepository.GetAllAsync();
    }

    public async Task<List<Post>> GetAllBlogPostsAsync()
    {
        return await postRepository.GetAllBlogPostsAsync();
    }

    public async Task<Post?> GetPostAsync(int id)
    {
        return await postRepository.GetByIdAsync(id);
    }

    public async Task<Post?> GetPostBySlugAsync(string slug)
    {
        return await postRepository.GetBySlugAsync(slug);
    }

    public async Task CreatePostAsync(Post post)
    {
        post.CreatedAt = DateTime.UtcNow;
        post.Summary = GenerateSummary(post);
        post.Slug = post.Title.Slugify();
        await postRepository.AddAsync(post);
    }

    public async Task UpdatePostAsync(Post post)
    {
        post.Summary = GenerateSummary(post);
        post.Slug = post.Title.Slugify();
        await postRepository.UpdateAsync(post);
    }

    public async Task RemovePostAsync(Post post)
    {
        await postRepository.DeleteAsync(post.Id);
    }

    public async Task<bool> PostHasValidSlug(Post post)
    {
        string slug = post.Title.Slugify();
        bool exists = await postRepository.SlugExistsAsync(slug, post.Id);
        return !exists;
    }

    public async Task IncrementVisitorCountAsync(int postId)
    {
        await postRepository.IncrementVisitorCountAsync(postId);
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
