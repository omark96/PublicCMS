using PublicCMS.Models;
using PublicCMS.Repositories;

namespace PublicCMS.Services;

public class NavMenuService
{
    private readonly NavMenuRepository navMenuRepository;

    public NavMenuService(NavMenuRepository navMenuRepository)
    {
        this.navMenuRepository = navMenuRepository;
    }

    public async Task<List<NavMenuItem>> GetMenuItemsAsync()
    {
        return await navMenuRepository.GetMenuItemsAsync();
    }

    public async Task SaveMenuItemsAsync(List<NavMenuItem> menuItems)
    {
        for (var i = 0; i < menuItems.Count; i++)
        {
            menuItems[i].Order = i;
        }

        await navMenuRepository.SaveMenuItemsAsync(menuItems);
    }
}
