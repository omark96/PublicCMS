using Microsoft.EntityFrameworkCore;
using PublicCMS.Data;
using PublicCMS.Models;

namespace PublicCMS.Services;

public class NavMenuService
{
    private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

    public NavMenuService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public async Task<List<NavMenuItem>> GetMenuItemsAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.NavMenuItems
            .OrderBy(n => n.Order)
            .ToListAsync();
    }

    public async Task SaveMenuItems(List<NavMenuItem> menuItems)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var previousItems = await context.NavMenuItems.ToListAsync();
        var incomingIds = menuItems.Select(i => i.Id).ToHashSet();

        context.RemoveRange(previousItems.Where(p => !incomingIds.Contains(p.Id)));

        foreach (var (item, index) in menuItems.Select((item, index) => (item, index)))
        {
            item.Order = index;
            var match = previousItems.FirstOrDefault(p => p.Id == item.Id);
            if (match is null)
            {
                context.NavMenuItems.Add(item);
            }
            else
            {
                context.Entry(match).CurrentValues.SetValues(item);
            }
        }

        await context.SaveChangesAsync();
    }
}
