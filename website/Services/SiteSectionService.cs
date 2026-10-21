using PublicCMS.Models;
using PublicCMS.Repositories;

namespace PublicCMS.Services;

public class SiteSectionService
{
    private readonly SiteSectionRepository siteSectionRepository;

    public SiteSectionService(SiteSectionRepository siteSectionRepository)
    {
        this.siteSectionRepository = siteSectionRepository;
    }

    public async Task<List<SiteSection>> GetSiteSectionsAsync()
    {
        return await siteSectionRepository.GetAllAsync();
    }

    public async Task<SiteSection?> GetSiteSectionByNameAsync(string name)
    {
        return await siteSectionRepository.GetByNameAsync(name);
    }

    public async Task<SiteSection> CreateSiteSectionAsync()
    {
        SiteSection section = new() { Name = Guid.NewGuid().ToString() };
        await siteSectionRepository.AddAsync(section);
        return section;
    }

    public async Task SaveSiteSectionAsync(SiteSection section)
    {
        await siteSectionRepository.UpdateAsync(section);
    }

    public async Task RemoveSiteSectionAsync(SiteSection section)
    {
        await siteSectionRepository.RemoveAsync(section);
    }

    public async Task<bool> SiteSectionHasValidName(SiteSection section)
    {
        bool existing = await siteSectionRepository.NameExistsAsync(section.Name, section.Id);
        return !existing;
    }
}
