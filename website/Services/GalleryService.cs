using Microsoft.AspNetCore.Components.Forms;
using PublicCMS.Models;
using System.Net.Http.Headers;

namespace PublicCMS.Services;

public class GalleryService
{
    private readonly IHttpClientFactory httpClientFactory;

    public GalleryService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<List<Gallery>> GetGalleriesAsync()
    {
        var client = httpClientFactory.CreateClient("Api");
        return await client.GetFromJsonAsync<List<Gallery>>("api/galleries") ?? [];

    }

    public async Task<Gallery> CreateGalleryAsync(string name)
    {
        var client = httpClientFactory.CreateClient("Api");
        var response = await client.PostAsJsonAsync("api/galleries", new { Name = name });

        response.EnsureSuccessStatusCode();

        var gallery = await response.Content.ReadFromJsonAsync<Gallery>();

        return gallery ?? throw new InvalidOperationException("API returned an empty response");
    }

    public async Task<List<GalleryImage>> GetImagesAsync(Guid guid)
    {
        var client = httpClientFactory.CreateClient("Api");
        return await client.GetFromJsonAsync<List<GalleryImage>>($"/api/galleries/{guid}/images") ?? [];

    }

    public async Task<List<GalleryImage>> UploadImagesAsync(Guid guid, IReadOnlyList<IBrowserFile> files)
    {
        var client = httpClientFactory.CreateClient("Api");

        using var form = new MultipartFormDataContent();
        foreach (var file in files)
        {
            var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 20 * 1024 * 1024));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            form.Add(fileContent, "files", file.Name);
        }
        var response = await client.PostAsync($"api/galleries/{guid}/images", form);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<GalleryImage>>() ?? [];
    }
}
