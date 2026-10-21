using PublicCMS_API.Models;
using PublicCMS_API.Repositories;

namespace PublicCMS_API.Services;

public class GalleryService
{
    private readonly GalleryRepository galleryRepository;
    private readonly IWebHostEnvironment env;

    public GalleryService(GalleryRepository galleryRepository, IWebHostEnvironment env)
    {
        this.galleryRepository = galleryRepository;
        this.env = env;
    }

    public async Task<List<GalleryDto>> GetGalleriesAsync()
    {
        return await galleryRepository.GetAllAsync();
    }

    public async Task<GalleryDto> CreateGalleryAsync(string name)
    {
        Gallery gallery = new()
        {
            Name = name,
            Guid = Guid.NewGuid()
        };
        await galleryRepository.AddAsync(gallery);

        return new GalleryDto(
            gallery.Guid,
            gallery.Name,
            []
            );
    }

    public async Task<GalleryDto?> GetGalleryAsync(Guid galleryGuid)
    {
        return await galleryRepository.GetDtoByGuidAsync(galleryGuid);
    }

    public async Task<List<GalleryImage>> GetImagesAsync(Guid galleryId)
    {
        return await galleryRepository.GetImagesAsync(galleryId);
    }

    public async Task<GalleryImage> UploadImageAsync(Guid galleryGuid, Stream stream, string fileName)
    {
        List<String> allowedExtensions = [".jpg", ".png", ".jpeg", ".gif", ".webp"];
        var gallery = await galleryRepository.GetByGuidAsync(galleryGuid);
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
            GalleryId = gallery.Id
        };

        try
        {
            await galleryRepository.AddImageAsync(image);
        }
        catch
        {
            File.Delete(fullPath);
        }

        return image;
    }

    internal async Task<GalleryImage?> GetImageAsync(Guid imageId)
    {
        return await galleryRepository.GetImageAsync(imageId);
    }

    internal async Task RemoveGalleryAsync(Guid galleryId)
    {
        await galleryRepository.RemoveAsync(galleryId);

        var galleryPath = Path.Combine(env.WebRootPath, "uploads", galleryId.ToString());
        if (Directory.Exists(galleryPath))
        {
            Directory.Delete(galleryPath, recursive: true);
        }
    }

    internal async Task<GalleryDto?> UpdateGalleryAsync(Guid galleryId, string name)
    {
        return await galleryRepository.UpdateNameAsync(galleryId, name);
    }

    internal async Task RemoveImageAsync(Guid imageGuid)
    {
        GalleryImage? image = await galleryRepository.GetImageAsync(imageGuid);
        if (image is null)
        {
            return;
        }

        await galleryRepository.RemoveImageAsync(imageGuid);

        var fullPath = Path.Combine(env.WebRootPath, "uploads", image.Path);
        File.Delete(fullPath);
    }
}
