using System.Collections.ObjectModel;
using Duende.IdentityServer.Extensions;
using Inventory.Models;
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

        public async Task<IEnumerable<Item>> GetAllItemsBySearchStringAsync(string searchString, int page, string? type)
        {
            try
            {
                return await _context.Items
                    .Include(c => c.ItemTemplate)
                    .ThenInclude(c => c.Category)
                    .Where(c => (c.WpId.Contains(searchString) || c.SerialNumber.Contains(searchString) 
                                                               || c.ItemTemplate.Description.Contains(searchString))
                                && (type.IsNullOrEmpty() || c.ItemTemplate.Type == type))
                    .Include(c => c.Parent)
                    .Include(c => c.Children)
                    .Include(c => c.CreatedBy)
                    .Include(c => c.Vendor)
                    .Include(c => c.Location)
                    .Include(c => c.LogEntries)
                    .ThenInclude(c => c.CreatedBy)
                    .OrderBy(c => c.Id)
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
        
        public async Task<IEnumerable<Item>> GetAllItemsNotInListBySearchStringAsync(string searchString, string listId, int page)
        {
            try
            {
                return await _context.Items.Include(c => c.ItemTemplate)
                    .ThenInclude(c => c.Category)
                    .Where(c => c.WpId.Contains(searchString) || c.SerialNumber.Contains(searchString)
                                                              || c.ItemTemplate.Description.Contains(searchString)
                                                              && c.ListId != listId)
                    .Include(c => c.Parent)
                    .Include(c => c.Children)
                    .Include(c => c.CreatedBy)
                    .Include(c => c.Vendor)
                    .Include(c => c.Location)
                    .Include(c => c.LogEntries)
                    .ThenInclude(c => c.CreatedBy)
                    .OrderBy(c => c.Id)
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
        
        public async Task<IEnumerable<Item>> GetAllItemsByUserIdAsync(string id, int page)
        {
            try
            {
                return await _context.Items.Where(c => c.CreatedById == id)
                    .Include(c => c.ItemTemplate)
                    .ThenInclude(c => c.Category)
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
                    .ThenInclude(c => c.Category)
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

        public async Task<Item> GetItemByIdAsync(string id)
        {
            try
            {
                return await _context.Items
                    .Include(c => c.ItemTemplate)
                    .ThenInclude(c => c.Category)
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
        
        public async Task<List<string>?> CreateItemAsync(List<Item> itemsCreate)
        {
            try
            {
                List<string> createdItemIds = new List<string>();
                foreach (var item in itemsCreate)
                {
                   item.CreatedDate = DateTime.Now;
                    await _context.Items.AddAsync(item);
                    createdItemIds.Add(item.Id);

                    LogEntry logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        CreatedById = item.CreatedById,
                        Message = "Item added",
                        CreatedDate = item.CreatedDate
                    };

                    await _context.LogEntries.AddAsync(logEntry);
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
                var item = await _context.Items.FirstOrDefaultAsync(c => c.Id == updatedItem.Id);

                if (item != null)
                {
                    LogEntry logEntry;
                    if (updatedItem.WpId != item.WpId)
                    {
                        logEntry = new LogEntry
                        {
                            ItemId = item.Id,
                            CreatedById = updatedById,
                            Message = "WellPartner ID changed from " + item.WpId + " to " + updatedItem.WpId,
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
                            Message = "Serial number changed from " + item.SerialNumber + " to " +
                                      updatedItem.SerialNumber,
                            CreatedDate = DateTime.Now
                        };
                        item.SerialNumber = updatedItem.SerialNumber;
                        await _context.LogEntries.AddAsync(logEntry);
                    }

                    if (updatedItem.LocationId != item.LocationId && updatedItem.LocationId != null)
                    {
                        var location =
                            await _context.Locations.FirstOrDefaultAsync(c => c.Id == updatedItem.LocationId);
                        logEntry = new LogEntry
                        {
                            ItemId = item.Id,
                            CreatedById = updatedById,
                            Message = "Location changed from " + item.Location?.Name + " to " + location.Name,
                            CreatedDate = DateTime.Now
                        };
                        item.LocationId = updatedItem.LocationId;
                        await _context.LogEntries.AddAsync(logEntry);
                    }

                    if (updatedItem.ParentId != item.ParentId)
                    {
                        logEntry = new LogEntry
                        {
                            ItemId = item.Id,
                            CreatedById = updatedById,
                            Message = "Parent ID changed from " + item.WpId + " to " + updatedItem.WpId,
                            CreatedDate = DateTime.Now
                        };
                        item.ParentId = updatedItem.ParentId;
                        await _context.LogEntries.AddAsync(logEntry);
                    }

                    if (updatedItem.Children != null) {
                        foreach (var child in updatedItem.Children) 
                        {
                            if (!item.Children.Any(c => c.Id == child.Id)) 
                            {
                                logEntry = new LogEntry
                                {
                                    ItemId = item.Id,
                                    CreatedById = updatedById,
                                    Message = $"Child item added: {child.WpId}",
                                    CreatedDate = DateTime.Now
                                };
                                await _context.LogEntries.AddAsync(logEntry);
                            }
                            
                        }
                    }

                    
                    if (item.Children != null) {
                        foreach (var child in item.Children) 
                        {
                            if (!updatedItem.Children.Any(c => c.Id == child.Id)) 
                            {
                                logEntry = new LogEntry
                                {
                                    ItemId = item.Id,
                                    CreatedById = updatedById,
                                    Message = $"Child item removed: {child.WpId}",
                                    CreatedDate = DateTime.Now
                                };
                                await _context.LogEntries.AddAsync(logEntry);
                            }
                            
                        }
                    }


                    if (updatedItem.VendorId != item.VendorId && updatedItem.VendorId != null)
                    {
                        var vendor = await _context.Vendors.FirstOrDefaultAsync(c => c.Id == updatedItem.VendorId);
                        logEntry = new LogEntry
                        {
                            ItemId = item.Id,
                            CreatedById = updatedById,
                            Message = "Vendor changed from " + item.Vendor?.Name + " to " + vendor.Name,
                            CreatedDate = DateTime.Now
                        };
                        item.VendorId = updatedItem.VendorId;
                        await _context.LogEntries.AddAsync(logEntry);
                    }

                    item.CreatedById = updatedItem.CreatedById;
                    item.Comment = updatedItem.Comment;
                    item.ListId = updatedItem.ListId;
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
                var parentItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == parentItemId);
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

        public async Task DeleteItemAsync(string id)
        {
            try
            {
                var item = await _context.Items.Include(c => c.Children).FirstOrDefaultAsync(c => c.Id == id);
                if (item != null)
                {
                    foreach (var child in item.Children)
                    {
                        child.ParentId = null;
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
    }
}
