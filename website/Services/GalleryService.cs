using Microsoft.AspNetCore.Components.Forms;
using PublicCMS.Models;
using PublicCMS.Repositories;

namespace PublicCMS.Services;

public class GalleryService
{
    private readonly GalleryRepository galleryRepository;

    public GalleryService(GalleryRepository galleryRepository)
    {
        this.galleryRepository = galleryRepository;
    }

    public async Task<List<Gallery>> GetGalleriesAsync()
    {
        return await galleryRepository.GetGalleriesAsync();

    }

    public async Task<Gallery?> GetGalleryAsync(Guid guid)
    {
        return await galleryRepository.GetGalleryAsync(guid);
    }

    public async Task<Gallery?> CreateGalleryAsync(string name)
    {
        return await galleryRepository.CreateGalleryAsync(name);
    }
    internal async Task RemoveGalleryAsync(Guid guid)
    {
        await galleryRepository.RemoveGalleryAsync(guid);
    }

    public async Task<Gallery?> UpdateGalleryAsync(Gallery gallery)
    {
        return await galleryRepository.UpdateGalleryAsync(gallery);
    }

    public async Task<List<GalleryImage>> GetImagesAsync(Guid guid)
    {
        return await galleryRepository.GetImagesAsync(guid);
    }

    public async Task<List<GalleryImage>> UploadImagesAsync(Guid guid, IReadOnlyList<IBrowserFile> files)
    {
        List<GalleryImage> uploaded = [];

        foreach (var file in files)
        {
            var image = await galleryRepository.UploadImageAsync(guid, file);
            if (image is not null)
            {
                uploaded.Add(image);
            }
        }
        return uploaded;
    }

    internal async Task RemoveImageAsync(Guid guid)
    {
        await galleryRepository.RemoveImageAsync(guid);
    }

}
