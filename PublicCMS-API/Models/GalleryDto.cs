namespace PublicCMS_API.Models;

public record GalleryDto(Guid Guid, string Name, List<GalleryImageDto> Images);
