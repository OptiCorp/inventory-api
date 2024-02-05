using System.Collections.ObjectModel;
using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services
{
    public class ItemService : IItemService
    {
        private readonly InventoryDbContext _context;

        public ItemService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            try
            {
                return await _context.Items
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
                var result = await _context.Items
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

                var templateIds = await _context.ItemTemplates
                    .Where(c => c.Description != null && c.Description.Contains(searchString))
                    .OrderBy(c => c.Id)
                    .Select(c => c.Id)
                    .ToListAsync();

                var items = await _context.Items
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
                return await _context.Items.Where(c => c.CreatedById == id)
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
                return await _context.Items.Where(c => c.ParentId == parentId)
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
                return await _context.Items
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

        public async Task<List<string>?> CreateItemAsync(List<ItemCreateDto> itemsCreate)
        {
            try
            {
                List<string> createdItemIds = new List<string>();
                foreach (var itemCreate in itemsCreate)
                {
                    Item item = new Item
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
                    };
                    await _context.Items.AddAsync(item);
                    if (item.Id != null)
                    {
                        createdItemIds.Add(item.Id);

                        LogEntry logEntry = new LogEntry
                        {
                            ItemId = item.Id,
                            CreatedById = item.CreatedById,
                            Message = "Item added",
                            CreatedDate = item.CreatedDate
                        };

                        await _context.LogEntries.AddAsync(logEntry);
                    }

                    await _context.SaveChangesAsync();
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
                var item = await _context.Items
                    .Include(item => item.Location)
                    .Include(item => item.Vendor)
                    .FirstOrDefaultAsync(c => c.Id == updatedItem.Id);

                if (item != null)
                {
                    LogEntry logEntry;
                    if (updatedItem.WpId != item.WpId)
                    {
                        logEntry = new LogEntry
                        {
                            ItemId = item.Id,
                            CreatedById = updatedById,
                            Message = $"WellPartner ID changed from {item.WpId} to {updatedItem.WpId}",
                            CreatedDate = DateTime.Now
                        };
                        item.WpId = updatedItem.WpId;
                        await _context.LogEntries.AddAsync(logEntry);
                    }

                    if (updatedItem.SerialNumber != item.SerialNumber)
                    {
                        logEntry = new LogEntry
                        {
                            ItemId = item.Id,
                            CreatedById = updatedById,
                            Message = $"Serial number changed from {item.SerialNumber} to {updatedItem.SerialNumber}",
                            CreatedDate = DateTime.Now
                        };
                        item.SerialNumber = updatedItem.SerialNumber;
                        await _context.LogEntries.AddAsync(logEntry);
                    }

                    if (updatedItem.LocationId != item.LocationId && updatedItem.LocationId != null)
                    {
                        var location = await _context.Locations.FirstOrDefaultAsync(c => c.Id == updatedItem.LocationId);
                        logEntry = new LogEntry
                        {
                            ItemId = item.Id,
                            CreatedById = updatedById,
                            Message = $"Location changed from {item.Location?.Name} to {location?.Name}",
                            CreatedDate = DateTime.Now
                        };
                        item.LocationId = updatedItem.LocationId;
                        await _context.LogEntries.AddAsync(logEntry);
                    }

                    if (updatedItem.ParentId != item.ParentId)
                    {
                        var oldParent = await _context.Items.FirstOrDefaultAsync(c => c.Id == item.ParentId);
                        var newParent = await _context.Items.FirstOrDefaultAsync(c => c.Id == updatedItem.ParentId);

                        logEntry = new LogEntry
                        {
                            ItemId = item.Id,
                            CreatedById = updatedById,
                            Message = $"Parent ID changed from {oldParent?.WpId} to {newParent?.WpId}",
                            CreatedDate = DateTime.Now
                        };

                        item.ParentId = updatedItem.ParentId;
                        await _context.LogEntries.AddAsync(logEntry);

                        logEntry = new LogEntry()
                        {
                            ItemId = oldParent?.Id,
                            CreatedById = updatedById,
                            Message = $"Item {updatedItem.Id} removed from parent {oldParent?.Id}",
                            CreatedDate = DateTime.Now
                        };

                        await _context.LogEntries.AddAsync(logEntry);

                        logEntry = new LogEntry()
                        {
                            ItemId = newParent?.Id,
                            CreatedById = updatedById,
                            Message = $"Item {updatedItem.Id} added to parent {newParent?.Id}",
                            CreatedDate = DateTime.Now
                        };

                        await _context.LogEntries.AddAsync(logEntry);
                    }

                    if (updatedItem.VendorId != item.VendorId && updatedItem.VendorId != null)
                    {
                        var vendor = await _context.Vendors.FirstOrDefaultAsync(c => c.Id == updatedItem.VendorId);
                        logEntry = new LogEntry
                        {
                            ItemId = item.Id,
                            CreatedById = updatedById,
                            Message = $"Vendor changed from {item.Vendor?.Name} to {vendor?.Name}",
                            CreatedDate = DateTime.Now
                        };
                        item.VendorId = updatedItem.VendorId;
                        await _context.LogEntries.AddAsync(logEntry);
                    }

                    item.CreatedById = updatedItem.CreatedById;
                    item.Comment = updatedItem.Comment;
                    item.UpdatedDate = DateTime.Now;

                    await _context.SaveChangesAsync();
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
                var parentItem = await _context.Items.Include(item => item.Children).FirstOrDefaultAsync(i => i.Id == parentItemId);
                var childItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == childItemId);

                if (parentItem != null && childItem != null)
                {
                    if (parentItem.Children == null)
                    {
                        parentItem.Children = new Collection<Item>();
                    }
                    parentItem.Children.Add(childItem);
                    await _context.SaveChangesAsync();
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
                var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemId);

                if (item != null)
                {
                    item.ParentId = null;
                    await _context.SaveChangesAsync();
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
                var item = await _context.Items.Include(c => c.Children).FirstOrDefaultAsync(c => c.Id == id);
                if (item != null)
                {
                    if (item.Children != null)
                        foreach (var child in item.Children)
                        {
                            child.ParentId = null;
                            if (deleteSubItems != true) continue;
                            if (child.Id != null) await DeleteItemAsync(child.Id, deleteSubItems);
                        }

                    _context.Items.Remove(item);
                    await _context.SaveChangesAsync();
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
                var item = await _context.Items.FirstOrDefaultAsync(c => c.WpId == id);
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
                var item = await _context.Items.FirstOrDefaultAsync(c => c.SerialNumber == serialNumber);
                return item == null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
