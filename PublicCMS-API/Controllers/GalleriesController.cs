using Microsoft.AspNetCore.Mvc;
using PublicCMS_API.Models;
using PublicCMS_API.Services;

namespace PublicCMS_API.Controllers;



[Route("api/[controller]")]
[ApiController]
public class GalleriesController : ControllerBase
{
    private readonly GalleryService galleryService;

    public GalleriesController(GalleryService galleryService)
    {
        this.galleryService = galleryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Gallery>>> GetGalleries()
    {
        return await galleryService.GetGalleriesAsync();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Gallery gallery)
    {
        Gallery newGallery = await galleryService.CreateGalleryAsync(gallery.Name);

        return Created($"api/galleries/{newGallery.Id}", newGallery);
    }

    [HttpGet("{galleryId:guid}/images")]
    public async Task<ActionResult<List<ImageInfo>>> GetImages(Guid galleryId)
    {
        var images = await galleryService.GetImagesAsync(galleryId);
        return images.Select(i => new ImageInfo(i.Guid, i.Name)).ToList();
    }

    [HttpPost("{galleryId:guid}/images")]
    [RequestSizeLimit(50 * 1024 * 1024)]
    public async Task<IActionResult> UploadImages(Guid galleryId, List<IFormFile> files)
    {
        var created = new List<ImageInfo>();
        foreach (var file in files)
        {
            await using var stream = file.OpenReadStream();
            var image = await galleryService.UploadImageAsync(galleryId, stream, file.FileName);
            created.Add(new ImageInfo(image.Guid, image.Name));
        }
        return Ok(created);
    }
}
