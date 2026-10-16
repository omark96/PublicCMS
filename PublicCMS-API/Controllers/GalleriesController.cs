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
}
