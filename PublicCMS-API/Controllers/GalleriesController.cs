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
    public async Task<ActionResult<List<GalleryDto>>> GetGalleries()
    {
        return await galleryService.GetGalleriesAsync();
    }

    [HttpGet("{galleryId:guid}")]
    public async Task<ActionResult<GalleryDto>> GetGallery(Guid galleryId)
    {
        var gallery = await galleryService.GetGalleryAsync(galleryId);
        if (gallery is null)
        {
            return NotFound();
        }
        return gallery;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGalleryRequest request)
    {
        var newGallery = await galleryService.CreateGalleryAsync(request.Name);

        return Created($"api/galleries/{newGallery.Guid}", newGallery);
    }

    [HttpGet("{galleryId:guid}/images")]
    public async Task<ActionResult<List<GalleryImageDto>>> GetImages(Guid galleryId)
    {
        var images = await galleryService.GetImagesAsync(galleryId);
        return images.Select(i => new GalleryImageDto(i.Guid, i.Name)).ToList();
    }

    [HttpPost("{galleryId:guid}/images")]
    [RequestSizeLimit(50 * 1024 * 1024)]
    public async Task<IActionResult> UploadImages(Guid galleryId, List<IFormFile> files)
    {
        var created = new List<GalleryImageDto>();
        foreach (var file in files)
        {
            await using var stream = file.OpenReadStream();
            var image = await galleryService.UploadImageAsync(galleryId, stream, file.FileName);
            created.Add(new GalleryImageDto(image.Guid, image.Name));
        }
        return Ok(created);
    }
}
