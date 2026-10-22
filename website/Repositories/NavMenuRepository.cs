using Microsoft.EntityFrameworkCore;
using PublicCMS.Data;
using PublicCMS.Models;

namespace PublicCMS.Repositories;

// The project originally only used Services, but as I read the spec it required it to be split into both services and repositories.
// AI was used to generate the split and no new functionality (except for where it was required based on the split) was added in this commit.
// See the following commit for the split: https://github.com/omark96/PublicCMS/commit/d482e3fbc8f4ad34076ceea2caa3efb6e6eeaecb
public class NavMenuRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> contextFactory;

    public NavMenuRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
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

    public async Task SaveMenuItemsAsync(IReadOnlyList<NavMenuItem> menuItems)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var previousItems = await context.NavMenuItems.ToListAsync();
        var incomingIds = menuItems.Select(i => i.Id).ToHashSet();

        context.RemoveRange(previousItems.Where(p => !incomingIds.Contains(p.Id)));

        var existing = previousItems.ToDictionary(p => p.Id);
        foreach (var item in menuItems)
        {
            if (existing.TryGetValue(item.Id, out var match))
                context.Entry(match).CurrentValues.SetValues(item);
            else
                context.NavMenuItems.Add(item);
        }

        await context.SaveChangesAsync();
    }
}
