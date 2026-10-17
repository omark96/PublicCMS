namespace PublicCMS.Data;

public class ApiSettings
{
    public string BaseUrl { get; set; } = "";
    public string ImageUrl(Guid id) => $"{BaseUrl.TrimEnd('/')}/api/images/{id}";
}
