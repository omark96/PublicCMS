using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Forms;
using PublicCMS.Models;

namespace PublicCMS.Repositories;

// The project originally only used Services, but as I read the spec it required it to be split into both services and repositories.
// AI was used to generate the split and no new functionality (except for where it was required based on the split) was added in this commit.
// See the following commit for the split: https://github.com/omark96/PublicCMS/commit/d482e3fbc8f4ad34076ceea2caa3efb6e6eeaecb
public class GalleryRepository
{
    private readonly IHttpClientFactory httpClientFactory;

    public GalleryRepository(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<List<Gallery>> GetGalleriesAsync()
    {
        var client = httpClientFactory.CreateClient("Api");
        return await client.GetFromJsonAsync<List<Gallery>>("api/galleries") ?? [];

    }

    public async Task<Gallery?> GetGalleryAsync(Guid guid)
    {
        var client = httpClientFactory.CreateClient("Api");
        return await client.GetFromJsonAsync<Gallery>($"api/galleries/{guid}");
    }

    public async Task<Gallery?> CreateGalleryAsync(string name)
    {
        var client = httpClientFactory.CreateClient("Api");
        var response = await client.PostAsJsonAsync("api/galleries", new { Name = name });

        response.EnsureSuccessStatusCode();

        var gallery = await response.Content.ReadFromJsonAsync<Gallery>();

        return gallery;
    }
    internal async Task RemoveGalleryAsync(Guid guid)
    {
        var client = httpClientFactory.CreateClient("Api");

        await client.DeleteAsync($"api/galleries/{guid}");
    }

    public async Task<Gallery?> UpdateGalleryAsync(Gallery gallery)
    {
        var client = httpClientFactory.CreateClient("Api");
        var response = await client.PutAsJsonAsync($"api/galleries/{gallery.Guid}", new { Name = gallery.Name });

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<Gallery>();
    }

    public async Task<List<GalleryImage>> GetImagesAsync(Guid guid)
    {
        var client = httpClientFactory.CreateClient("Api");
        return await client.GetFromJsonAsync<List<GalleryImage>>($"/api/galleries/{guid}/images") ?? [];

    }

    // This was planned to be done through a single upload with multiple images at once.
    // However I couldn't get it to work properly so I switched to uploading each image individually.
    // That's why the methods look weird uploading a single image as a list of images.
    public async Task<GalleryImage?> UploadImageAsync(Guid guid, IBrowserFile file)
    {
        var client = httpClientFactory.CreateClient("Api");

        using var form = new MultipartFormDataContent();

        var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 20 * 1024 * 1024));
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
        form.Add(fileContent, "files", file.Name);

        var response = await client.PostAsync($"api/galleries/{guid}/images", form);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        List<GalleryImage> images = await response.Content.ReadFromJsonAsync<List<GalleryImage>>() ?? [];
        return images.FirstOrDefault();
    }

    internal async Task RemoveImageAsync(Guid guid)
    {
        var client = httpClientFactory.CreateClient("Api");

        await client.DeleteAsync($"api/images/{guid}");
    }
}
