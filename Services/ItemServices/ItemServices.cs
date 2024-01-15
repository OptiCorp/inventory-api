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
            return await _context.Items
                .Include(c => c.Children)
                .Include(c => c.CreatedBy)
                .Include(c => c.Vendor)
                .Include(c => c.Category)
                .Include(c => c.Location)
                .Include(c => c.LogEntries)!
                .ThenInclude(c => c.CreatedBy)
                .ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetAllItemsBySearchStringAsync(string searchString, int page, string? type)
        {   
            return await _context.Items
                            .Include(c => c.ItemTemplate)
                            .Where(c => (c.WpId.Contains(searchString) || c.SerialNumber.Contains(searchString) 
                            || c.ItemTemplate.Description.Contains(searchString))
                            && (type.IsNullOrEmpty() || c.ItemTemplate.Type == type))
                            .Include(c => c.Children)
                            .Include(c => c.CreatedBy)
                            .Include(c => c.Vendor)
                            .Include(c => c.Category)
                            .Include(c => c.Location)
                            .Include(c => c.LogEntries)
                            .ThenInclude(c => c.CreatedBy)
                            .OrderBy(c => c.Id)
                            .Skip(page == 0 ? 0 : (page - 1) * 10)
                            .Take(10)
                            .ToListAsync();
        }
        
        public async Task<IEnumerable<Item>> GetAllItemsNotInListBySearchStringAsync(string searchString, string listId, int page)
        {   
            return await _context.Items.Include(c => c.ItemTemplate)
                    .Where(c => c.WpId.Contains(searchString) || c.SerialNumber.Contains(searchString)
                    || c.ItemTemplate.Description.Contains(searchString)
                    && c.ListId != listId)
                    .Include(c => c.Children)
                    .Include(c => c.CreatedBy)
                    .Include(c => c.Vendor)
                    .Include(c => c.Category)
                    .Include(c => c.Location)
                    .Include(c => c.LogEntries)
                    .ThenInclude(c => c.CreatedBy)
                    .OrderBy(c => c.Id)
                    .Skip(page == 0 ? 0 : (page - 1) * 10)
                    .Take(10)
                    .ToListAsync();
        }
        
        public async Task<IEnumerable<Item>> GetAllItemsByUserIdAsync(string id, int page)
        {
            return await _context.Items.Where(c => c.CreatedById == id)
                .Include(c => c.Children)
                .Include(c => c.CreatedBy)
                .Include(c => c.Vendor)
                .Include(c => c.Category)
                .Include(c => c.Location)
                .Include(c => c.LogEntries)!
                .ThenInclude(c => c.CreatedBy)
                .OrderByDescending(c => c.CreatedDate)
                .Skip(page == 0 ? 0 : (page - 1) * 10)
                .Take(10)
                .ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetChildrenAsync(string parentId)
        {
            return await _context.Items.Where(c => c.ParentId == parentId)
                .Include(c => c.Children)
                .Include(c => c.CreatedBy)
                .Include(c => c.Vendor)
                .Include(c => c.Category)
                .Include(c => c.Location)
                .Include(c => c.LogEntries)!
                .ThenInclude(c => c.CreatedBy)
                .ToListAsync();
        }

        public async Task<Item> GetItemByIdAsync(string id)
        {
            return await _context.Items
                .Include(c => c.ItemTemplate)
                .Include(c => c.Documents)
                .Include(c => c.Children)
                .Include(c => c.CreatedBy)
                .Include(c => c.Vendor)
                .Include(c => c.Category)
                .Include(c => c.Location)
                .Include(c => c.LogEntries)!
                .ThenInclude(c => c.CreatedBy)
                .FirstOrDefaultAsync(c => c.Id == id);
            
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
                return null;
            }
        }
        
        public async Task UpdateItemAsync(string updatedById, Item itemUpdate)
        {
            var item = await _context.Items.Include(c => c.Category).FirstOrDefaultAsync(c => c.Id == itemUpdate.Id);
        
            if (item != null)
            {
                LogEntry logEntry;
                if (itemUpdate.WpId != item.WpId)
                {
                    logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        CreatedById = updatedById,
                        Message = "WellPartner ID changed from " + item.WpId + " to " + itemUpdate.WpId,
                        CreatedDate = DateTime.Now
                    };
                    item.WpId = itemUpdate.WpId;
                    await _context.LogEntries.AddAsync(logEntry);
                }
                
                if (itemUpdate.SerialNumber != item.SerialNumber)
                {
                    logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        CreatedById = updatedById,
                        Message = "Serial number changed from " + item.SerialNumber + " to " + itemUpdate.SerialNumber,
                        CreatedDate = DateTime.Now
                    };
                    item.SerialNumber = itemUpdate.SerialNumber;
                    await _context.LogEntries.AddAsync(logEntry);
                }
                
                if (itemUpdate.ItemTemplate.ProductNumber != item.ItemTemplate.ProductNumber)
                {
                    logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        CreatedById = updatedById,
                        Message = "Product number changed from " + item.ItemTemplate.ProductNumber + " to " + itemUpdate.ItemTemplate.ProductNumber,
                        CreatedDate = DateTime.Now
                    };
                    item.ItemTemplate.ProductNumber = itemUpdate.ItemTemplate.ProductNumber;
                    await _context.LogEntries.AddAsync(logEntry);
                }
                
                if (itemUpdate.ItemTemplate.Type != item.ItemTemplate.Type)
                {
                    logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        CreatedById = updatedById,
                        Message = "Type changed from " + item.ItemTemplate.Type + " to " + itemUpdate.ItemTemplate.Type,
                        CreatedDate = DateTime.Now
                    };
                    item.ItemTemplate.Type = itemUpdate.ItemTemplate.Type;
                    await _context.LogEntries.AddAsync(logEntry);
                }
                
                if (itemUpdate.CategoryId != item.CategoryId && itemUpdate.CategoryId != null)
                {
                    var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == itemUpdate.CategoryId);
                    logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        CreatedById = updatedById,
                        Message = "Category changed from " + item.Category?.Name + " to " + category.Name,
                        CreatedDate = DateTime.Now
                    };
                    item.CategoryId = itemUpdate.CategoryId;
                    await _context.LogEntries.AddAsync(logEntry);
                }
                
                if (itemUpdate.LocationId != item.LocationId && itemUpdate.LocationId != null)
                {
                    var location = await _context.Locations.FirstOrDefaultAsync(c => c.Id == itemUpdate.LocationId);
                    logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        CreatedById = updatedById,
                        Message = "Location changed from " + item.Location?.Name + " to " + location.Name,
                        CreatedDate = DateTime.Now
                    };
                    item.LocationId = itemUpdate.LocationId;
                    await _context.LogEntries.AddAsync(logEntry);
                }
               
                if (itemUpdate.ItemTemplate.Description != item.ItemTemplate.Description)
                {
                    logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        CreatedById = updatedById,
                        Message = "Description updated",
                        CreatedDate = DateTime.Now
                    };
                    item.ItemTemplate.Description = itemUpdate.ItemTemplate.Description;
                    await _context.LogEntries.AddAsync(logEntry);
                }
                
                if (itemUpdate.ParentId != item.ParentId)
                {
                    logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        CreatedById = updatedById,
                        Message = "Parent ID changed from " + item.ParentId + " to " + itemUpdate.ParentId,
                        CreatedDate = DateTime.Now
                    };
                    item.ParentId = itemUpdate.ParentId;
                    await _context.LogEntries.AddAsync(logEntry);
                }
                
                if (itemUpdate.VendorId != item.VendorId && itemUpdate.VendorId != null)
                {
                    var vendor = await _context.Vendors.FirstOrDefaultAsync(c => c.Id == itemUpdate.VendorId);
                    logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        CreatedById = updatedById,
                        Message = "Vendor changed from " + item.Vendor?.Name + " to " + vendor.Name,
                        CreatedDate = DateTime.Now
                    };
                    item.VendorId = itemUpdate.VendorId;
                    await _context.LogEntries.AddAsync(logEntry);
                }

                item.CreatedById = itemUpdate.CreatedById;
                item.Comment = itemUpdate.Comment;
                item.ListId = itemUpdate.ListId;
                item.UpdatedDate = DateTime.Now;
                
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteItemAsync(string id)
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
        
        public async Task<bool> IsWpIdUnique(string id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(c => c.WpId == id);
            return item == null;
        }
    }
}
