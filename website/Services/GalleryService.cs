using PublicCMS.Models;

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
}
