using Microsoft.EntityFrameworkCore;
using PublicCMS_API.Data;
using PublicCMS_API.Models;

namespace PublicCMS_API.Services;

public class GalleryService
{
    private readonly ApiDbContext context;

    public GalleryService(ApiDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Gallery>> GetGalleriesAsync()
    {
        return await context.Galleries.ToListAsync();
    }

    public async Task<Gallery> CreateGalleryAsync(string name)
    {
        Gallery gallery = new()
        {
            Name = name,
            Guid = Guid.NewGuid()
        };
        context.Galleries.Add(gallery);
        await context.SaveChangesAsync();

        return gallery;
    }
}
