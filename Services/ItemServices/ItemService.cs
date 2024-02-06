using System.Collections.ObjectModel;
using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services;

public class ItemService(InventoryDbContext context) : IItemService
{
    public async Task<IEnumerable<Item>> GetAllItemsAsync()
    {
        try
        {
            return await context.Items
                .Include(c => c.LogEntries)!
                .ThenInclude(c => c.CreatedBy)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Item>> GetAllItemsBySearchStringAsync(string searchString, int page)
    {
        try
        {
            var result = await context.Items
                .Where(c => c.SerialNumber != null && c.WpId != null && (c.WpId.Contains(searchString) || c.SerialNumber.Contains(searchString)))
                .Include(c => c.ItemTemplate)
                .ThenInclude(c => c!.LogEntries)
                .Include(c => c.ItemTemplate)
                .ThenInclude(c => c!.Category)
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .Include(c => c.CreatedBy)
                .Include(c => c.Vendor)
                .Include(c => c.Location)
                .Include(c => c.LogEntries)!
                .ThenInclude(c => c.CreatedBy)
                .OrderBy(c => c.Id)
                .Take(page * 10)
                .ToListAsync();

            if (result.Count >= page * 10)
            {
                return result;
            }

            var remainingItemsCount = page * 10 - result.Count;

            var templateIds = await context.ItemTemplates
                .Where(c => c.Description != null && c.Description.Contains(searchString))
                .OrderBy(c => c.Id)
                .Select(c => c.Id)
                .ToListAsync();

            var items = await context.Items
                .Where(c => templateIds.Contains(c.ItemTemplateId))
                .Include(c => c.ItemTemplate)
                .ThenInclude(c => c!.LogEntries)
                .Include(c => c.ItemTemplate)
                .ThenInclude(c => c!.Category)
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .Include(c => c.CreatedBy)
                .Include(c => c.Vendor)
                .Include(c => c.Location)
                .Include(c => c.LogEntries)!
                .ThenInclude(c => c.CreatedBy)
                .OrderBy(c => c.Id)
                .Take(remainingItemsCount)
                .ToListAsync();

            result.AddRange(items);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Item>> GetAllItemsByUserIdAsync(string id, int page)
    {
        try
        {
            return await context.Items.Where(c => c.CreatedById == id)
                .Include(c => c.ItemTemplate)
                .ThenInclude(c => c!.LogEntries)
                .Include(c => c.ItemTemplate)
                .ThenInclude(c => c!.Category)
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .Include(c => c.CreatedBy)
                .Include(c => c.Vendor)
                .Include(c => c.Location)
                .Include(c => c.LogEntries)!
                .ThenInclude(c => c.CreatedBy)
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

    public async Task<IEnumerable<Item>> GetChildrenAsync(string parentId)
    {
        try
        {
            return await context.Items.Where(c => c.ParentId == parentId)
                .Include(c => c.ItemTemplate)
                .ThenInclude(c => c!.LogEntries)
                .Include(c => c.ItemTemplate)
                .ThenInclude(c => c!.Category)
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .Include(c => c.CreatedBy)
                .Include(c => c.Vendor)
                .Include(c => c.Location)
                .Include(c => c.LogEntries)!
                .ThenInclude(c => c.CreatedBy)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Item?> GetItemByIdAsync(string id)
    {
        try
        {
            return await context.Items
                .Include(c => c.ItemTemplate)
                .ThenInclude(c => c!.LogEntries)
                .Include(c => c.ItemTemplate)
                .ThenInclude(c => c!.Category)
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .Include(c => c.CreatedBy)
                .Include(c => c.Vendor)
                .Include(c => c.Location)
                .Include(c => c.LogEntries)!
                .ThenInclude(c => c.CreatedBy)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public async Task<List<string>?> CreateItemAsync(IEnumerable<ItemCreateDto> itemsCreate)
    {
        try
        {
            var createdItemIds = new List<string>();
            foreach (var item in itemsCreate.Select(itemCreate => new Item
            {
                WpId = itemCreate.WpId,
                ParentId = itemCreate.ParentId,
                ItemTemplateId = itemCreate.ItemTemplateId,
                SerialNumber = itemCreate.SerialNumber,
                LocationId = itemCreate.LocationId,
                VendorId = itemCreate.VendorId,
                CreatedById = itemCreate.CreatedById,
                Comment = itemCreate.Comment,
                CreatedDate = DateTime.Now
            }))
            {
                await context.Items.AddAsync(item);
                if (item.Id != null)
                {
                    createdItemIds.Add(item.Id);
                    if (item.CreatedById != null)
                        await CreateLogEntryAsync(item.Id, item.CreatedById, "Item added");
                }

                await context.SaveChangesAsync();
            }
            return createdItemIds;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateItemAsync(string updatedById, Item updatedItem)
    {
        try
        {
            var item = await context.Items
                .Include(item => item.Location)
                .Include(item => item.Vendor)
                .FirstOrDefaultAsync(c => c.Id == updatedItem.Id);

            if (item?.Id != null)
            {
                if (updatedItem.WpId != item.WpId)
                {
                    item.WpId = updatedItem.WpId;
                    await CreateLogEntryAsync(item.Id, updatedById, $"WellPartner ID changed from {item.WpId} to {updatedItem.WpId}");
                }

                if (updatedItem.SerialNumber != item.SerialNumber)
                {
                    item.SerialNumber = updatedItem.SerialNumber;
                    await CreateLogEntryAsync(item.Id, updatedById, $"Serial number changed from {item.SerialNumber} to {updatedItem.SerialNumber}");
                }

                if (updatedItem.LocationId != item.LocationId && updatedItem.LocationId != null)
                {
                    var location = await context.Locations.FirstOrDefaultAsync(c => c.Id == updatedItem.LocationId);
                    item.LocationId = updatedItem.LocationId;
                    await CreateLogEntryAsync(item.Id, updatedById, $"Location changed from {item.Location?.Name} to {location?.Name}");
                }

                if (updatedItem.ParentId != item.ParentId)
                {
                    var oldParent = await context.Items.FirstOrDefaultAsync(c => c.Id == item.ParentId);
                    var newParent = await context.Items.FirstOrDefaultAsync(c => c.Id == updatedItem.ParentId);

                    item.ParentId = updatedItem.ParentId;
                    await CreateLogEntryAsync(item.Id, updatedById, $"Parent ID changed from {oldParent?.WpId} to {newParent?.WpId}");

                    if (oldParent?.Id != null)
                        await CreateLogEntryAsync(oldParent.Id, updatedById,
                            $"Item {updatedItem.Id} removed from parent");

                    if (newParent?.Id != null)
                        await CreateLogEntryAsync(newParent.Id, updatedById,
                            $"Item {updatedItem.Id} added to parent");
                }

                if (updatedItem.VendorId != item.VendorId && updatedItem.VendorId != null)
                {
                    var vendor = await context.Vendors.FirstOrDefaultAsync(c => c.Id == updatedItem.VendorId);
                    item.VendorId = updatedItem.VendorId;
                    await CreateLogEntryAsync(item.Id, updatedById, $"Vendor changed from {item.Vendor?.Name} to {vendor?.Name}");
                }

                item.CreatedById = updatedItem.CreatedById;
                item.Comment = updatedItem.Comment;
                item.UpdatedDate = DateTime.Now;

                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task AddChildItemToParentAsync(string parentItemId, string childItemId)
    {
        try
        {
            var parentItem = await context.Items.Include(item => item.Children).FirstOrDefaultAsync(i => i.Id == parentItemId);
            var childItem = await context.Items.FirstOrDefaultAsync(i => i.Id == childItemId);

            if (parentItem != null && childItem != null)
            {
                parentItem.Children ??= new Collection<Item>();
                parentItem.Children.Add(childItem);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task RemoveParentIdAsync(string itemId)
    {
        try
        {
            var item = await context.Items.FirstOrDefaultAsync(i => i.Id == itemId);

            if (item != null)
            {
                item.ParentId = null;
                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteItemAsync(string id, bool? deleteSubItems)
    {
        try
        {
            var item = await context.Items.Include(c => c.Children).FirstOrDefaultAsync(c => c.Id == id);
            if (item != null)
            {
                if (item.Children != null)
                    foreach (var child in item.Children)
                    {
                        child.ParentId = null;
                        if (deleteSubItems != true) continue;
                        if (child.Id != null) await DeleteItemAsync(child.Id, deleteSubItems);
                    }

                context.Items.Remove(item);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> IsWpIdUnique(string id)
    {
        try
        {
            var item = await context.Items.FirstOrDefaultAsync(c => c.WpId == id);
            return item == null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> IsSerialNumberUnique(string serialNumber)
    {
        try
        {
            var item = await context.Items.FirstOrDefaultAsync(c => c.SerialNumber == serialNumber);
            return item == null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task CreateLogEntryAsync(string itemId, string createdById, string message)
    {
        try
        {
            var logEntry = new LogEntry
            {
                ItemId = itemId,
                CreatedById = createdById,
                Message = message,
                CreatedDate = DateTime.Now
            };

            await context.LogEntries.AddAsync(logEntry);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}