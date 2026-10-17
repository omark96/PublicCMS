using Microsoft.EntityFrameworkCore;
using PublicCMS_API.Data;
using PublicCMS_API.Models;

namespace PublicCMS_API.Services;

public class GalleryService
{
    private readonly ApiDbContext context;
    private readonly IWebHostEnvironment env;

    public GalleryService(ApiDbContext context, IWebHostEnvironment env)
    {
        this.context = context;
        this.env = env;
    }

    public async Task<List<GalleryDto>> GetGalleriesAsync()
    {
        return await context.Galleries
            .Select(g => new GalleryDto(
                Guid: g.Guid,
                Name: g.Name,
                Images: new()
                ))
            .ToListAsync();
    }

    public async Task<GalleryDto> CreateGalleryAsync(string name)
    {
        Gallery gallery = new()
        {
            Name = name,
            Guid = Guid.NewGuid()
        };
        context.Galleries.Add(gallery);
        await context.SaveChangesAsync();

        return new GalleryDto(
            gallery.Guid,
            gallery.Name,
            []
            );
    }
    public async Task<GalleryDto?> GetGalleryAsync(Guid galleryGuid)
    {
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

    public async Task<List<GalleryImage>> GetImagesAsync(Guid galleryId)
    {
        var gallery = await context.Galleries
            .Include(g => g.Images)
            .FirstOrDefaultAsync(g => g.Guid == galleryId);

        return gallery?.Images.ToList() ?? [];
    }

    public async Task<GalleryImage> UploadImageAsync(Guid galleryGuid, Stream stream, string fileName)
    {
        List<String> allowedExtensions = [".jpg", ".png", ".jpeg", ".gif", ".webp"];
        var gallery = await context.Galleries.SingleOrDefaultAsync(g => g.Guid == galleryGuid);
        if (gallery is null)
        {
            throw new KeyNotFoundException($"Gallery {galleryGuid} not found");
        }
        var extension = Path.GetExtension(fileName);
        if (!allowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException($"File type '{extension}' is not allowed.");
        }

        var imageGuid = Guid.NewGuid();
        var relativePath = Path.Combine(galleryGuid.ToString(), $"{imageGuid}{extension}");

        var fullPath = Path.Combine(env.WebRootPath, "uploads", relativePath);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        await using (var fileStream = new FileStream(fullPath, FileMode.CreateNew))
        {
            await stream.CopyToAsync(fileStream);
        }

        var image = new GalleryImage
        {
            Guid = imageGuid,
            Name = Path.GetFileNameWithoutExtension(fileName),
            Path = relativePath,
            Gallery = gallery
        };

        try
        {
            context.Images.Add(image);
            await context.SaveChangesAsync();
        }
        catch
        {
            File.Delete(fullPath);
        }

        return image;
    }

    internal async Task<GalleryImage?> GetImageAsync(Guid imageId)
    {
        return await context.Images.FirstOrDefaultAsync(i => i.Guid == imageId);
    }
}
