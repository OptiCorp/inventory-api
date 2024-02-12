using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services;

public class ListService(InventoryDbContext context) : IListService
{
    public async Task<IEnumerable<List>> GetAllListsAsync()
    {
        try
        {
            return await context.Lists.Include(c => c.CreatedBy)
                .Include(c => c.Items)!
                .ThenInclude(c => c.ItemTemplate)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<List>> GetAllListsBySearchStringAsync(string searchString, int page, string userId)
    {
        try
        {
            return await context.Lists.Include(c => c.Items)!
                .ThenInclude(c => c.ItemTemplate)
                .Where(c => c.CreatedById == userId)
                .Where(list =>
                    list.Items != null && list.Title != null && (list.Title.Contains(searchString) ||
                                                                 list.Items.Any(item =>
                                                                     item.WpId != null && item.ItemTemplate != null && item.SerialNumber != null && item.ItemTemplate.Description != null && (item.WpId.Contains(searchString) ||
                                                                         item.SerialNumber.Contains(searchString) ||
                                                                         item.ItemTemplate.Description.Contains(searchString))
                                                                 )))
                .Include(c => c.CreatedBy)
                .OrderByDescending(c => c.CreatedDate)
                .Skip(page == 0 ? 0 : (page - 1) * 10)
                .Take(10)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<List>> GetAllListsByUserIdAsync(string id, int page)
    {
        try
        {
            return await context.Lists.Where(c => c.CreatedById == id)
                .Include(c => c.CreatedBy)
                .Include(c => c.Items)!
                .ThenInclude(c => c.ItemTemplate)
                .OrderByDescending(c => c.CreatedDate)
                .Skip(page == 0 ? 0 : (page - 1) * 10)
                .Take(10)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List?> GetListByIdAsync(string id)
    {
        try
        {
            return await context.Lists
                .Include(c => c.CreatedBy)
                .Include(c => c.Items)!
                .ThenInclude(c => c.Vendor)
                .Include(c => c.Items)!
                .ThenInclude(c => c.ItemTemplate)
                .ThenInclude(c => c!.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string?> CreateListAsync(ListCreateDto listCreate)
    {
        try
        {
            var list = new List
            {
                Title = listCreate.Title,
                CreatedById = listCreate.CreatedById,
                CreatedDate = DateTime.Now
            };

            await context.Lists.AddAsync(list);
            await context.SaveChangesAsync();
            return list.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task AddItemsToListAsync(IEnumerable<string> itemIds, string listId, bool? addSubItems)
    {
        try
        {
            var list = await context.Lists.FirstOrDefaultAsync(c => c.Id == listId);

            foreach (var itemId in itemIds)
            {
                var item = await context.Items.Include(item => item.Children).FirstOrDefaultAsync(c => c.Id == itemId);
                if (item != null) list?.Items?.Add(item);

                var subItemIds = new List<string>();

                if (addSubItems != true) continue;
                if (item?.Children != null)
                    foreach (var child in item.Children)
                    {
                        if (child.Id != null) subItemIds.Add(child.Id);
                    }

                await AddItemsToListAsync(subItemIds, listId, addSubItems);
            }

            if (list != null) list.UpdatedDate = DateTime.Now;

            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task RemoveItemsFromListAsync(IEnumerable<string> itemIds, string listId, bool? removeSubItems)
    {
        try
        {
            var list = await context.Lists.FirstOrDefaultAsync(c => c.Id == listId);

            foreach (var itemId in itemIds)
            {
                var item = await context.Items.Include(item => item.Children).FirstOrDefaultAsync(c => c.Id == itemId);
                if (item != null) list?.Items?.Remove(item);

                var subItemIds = new List<string>();

                if (removeSubItems != true) continue;
                if (item?.Children != null)
                    foreach (var child in item.Children)
                    {
                        if (child.Id != null) subItemIds.Add(child.Id);
                    }

                await RemoveItemsFromListAsync(subItemIds, listId, removeSubItems);
            }

            if (list != null) list.UpdatedDate = DateTime.Now;

            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateListAsync(List listUpdate)
    {
        try
        {
            var list = await context.Lists.FirstOrDefaultAsync(c => c.Id == listUpdate.Id);

            if (list != null)
            {
                list.Title = listUpdate.Title;
                list.UpdatedDate = DateTime.Now;

                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteListAsync(string id)
    {
        try
        {
            var list = await context.Lists.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == id);
            if (list != null)
            {
                context.Lists.Remove(list);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}