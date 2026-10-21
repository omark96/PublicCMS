using Microsoft.EntityFrameworkCore;
using PublicCMS_API.Data;
using PublicCMS_API.Models;

namespace PublicCMS_API.Repositories;

public class GalleryRepository
{
    private readonly IDbContextFactory<ApiDbContext> contextFactory;

    public GalleryRepository(IDbContextFactory<ApiDbContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public async Task<List<GalleryDto>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Galleries
            .Select(g => new GalleryDto(
                Guid: g.Guid,
                Name: g.Name,
                Images: new()
                ))
            .ToListAsync();
    }

    public async Task<GalleryDto?> GetDtoByGuidAsync(Guid galleryGuid)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var gallery = await context.Galleries
            .Where(g => g.Guid == galleryGuid)
            .Select(g => new GalleryDto(
                g.Guid,
                g.Name,
                g.Images.Select(i => new GalleryImageDto(i.Guid, i.Name)
                )
                .ToList()))
            .FirstOrDefaultAsync();

        return gallery;
    }

    public async Task<Gallery?> GetByGuidAsync(Guid galleryGuid)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Galleries.SingleOrDefaultAsync(g => g.Guid == galleryGuid);
    }

    public async Task<List<GalleryImage>> GetImagesAsync(Guid galleryId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var gallery = await context.Galleries
            .Include(g => g.Images)
            .FirstOrDefaultAsync(g => g.Guid == galleryId);

        return gallery?.Images.ToList() ?? [];
    }

    public async Task<GalleryImage?> GetImageAsync(Guid imageId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Images.FirstOrDefaultAsync(i => i.Guid == imageId);
    }

    public async Task AddAsync(Gallery gallery)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.Galleries.Add(gallery);
        await context.SaveChangesAsync();
    }

    public async Task AddImageAsync(GalleryImage image)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.Images.Add(image);
        await context.SaveChangesAsync();
    }

    public async Task<GalleryDto?> UpdateNameAsync(Guid galleryId, string name)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        Gallery? gallery = await context.Galleries
            .Include(g => g.Images)
            .FirstOrDefaultAsync(g => g.Guid == galleryId);
        if (gallery is null)
        {
            return null;
        }

        gallery.Name = name;
        context.Update(gallery);
        await context.SaveChangesAsync();
        return new GalleryDto(gallery.Guid,
            gallery.Name,
            gallery.Images
            .Select(i => new GalleryImageDto(i.Guid, i.Name))
            .ToList());
    }

    public async Task RemoveAsync(Guid galleryId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        Gallery? gallery = await context.Galleries.FirstOrDefaultAsync(g => g.Guid == galleryId);
        if (gallery is not null)
        {
            context.Remove(gallery);
            await context.SaveChangesAsync();
        }
    }

    public async Task RemoveImageAsync(Guid imageGuid)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        GalleryImage? image = await context.Images.FirstOrDefaultAsync(i => i.Guid == imageGuid);
        if (image is not null)
        {
            context.Remove(image);
            await context.SaveChangesAsync();
        }
    }
}
