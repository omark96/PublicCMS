using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using PublicCMS_API.Services;

namespace PublicCMS_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly GalleryService galleryService;
    private readonly IWebHostEnvironment env;

    public ImagesController(GalleryService galleryService, IWebHostEnvironment env)
    {
        this.galleryService = galleryService;
        this.env = env;
    }

    [HttpGet("{imageId:guid}")]
    public async Task<IActionResult> Get(Guid imageId)
    {
        var image = await galleryService.GetImageAsync(imageId);
        if (image is null)
        {
            return NotFound();
        }

        var path = Path.Combine(env.WebRootPath, "uploads", image.Path);
        FileExtensionContentTypeProvider contentTypeProvider = new();
        if (!contentTypeProvider.TryGetContentType(path, out var type))
        {
            type = "application/octet-stream";
        }
        return PhysicalFile(path, type);
    }

    [HttpDelete("{guid:guid}")]
    public async Task<ActionResult> RemoveImageAsync(Guid guid)
    {
        await galleryService.RemoveImageAsync(guid);
        return Ok();
    }
}
