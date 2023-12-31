using Duende.IdentityServer.Extensions;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Inventory.Models.DTOs.ItemDTOs;
using Inventory.Utilities;

namespace Inventory.Services
{
    public class ItemService : IItemService
    {
        private readonly InventoryDbContext _context;
        private readonly IItemUtilities _itemUtilities;

        public ItemService(InventoryDbContext context, IItemUtilities itemUtilities)
        {
            _context = context;
            _itemUtilities = itemUtilities;
        }

        public async Task<IEnumerable<ItemResponseDto>> GetAllItemsAsync()
        {
            return await _context.Items.Include(c => c.Parent)
                .Include(c => c.Children)
                .Include(c => c.User)
                .Include(c => c.Vendor)
                .Include(c => c.Category)
                .Include(c => c.Location)
                .Include(c => c.LogEntries)!
                .ThenInclude(c => c.User)
                .Select(c => _itemUtilities.ItemToResponseDto(c))
                                            .ToListAsync();
        }

        public async Task<IEnumerable<ItemResponseDto>> GetAllItemsBySearchStringAsync(string searchString, int page, string? type)
        {
            return await _context.Items.Where(c => (c.WpId.Contains(searchString) || c.SerialNumber.Contains(searchString)
                            || c.Description.Contains(searchString))
                            && (type.IsNullOrEmpty() || c.Type == type))
                            .Include(c => c.Parent)
                            .Include(c => c.Children)
                            .Include(c => c.User)
                            .Include(c => c.Vendor)
                            .Include(c => c.Category)
                            .Include(c => c.Location)
                            .Include(c => c.LogEntries)
                            .ThenInclude(c => c.User)
                            .OrderBy(c => c.Id)
                            .Skip(page == 0 ? 0 : (page - 1) * 10)
                            .Take(10)
                            .Select(c => _itemUtilities.ItemToResponseDto(c))
                            .ToListAsync();
        }

        public async Task<IEnumerable<ItemResponseDto>> GetAllItemsNotInListBySearchStringAsync(string searchString, string listId, int page)
        {
            return await _context.Items.Where(c => c.WpId.Contains(searchString) || c.SerialNumber.Contains(searchString)
                    || c.Description.Contains(searchString)
                    && c.ListId != listId)
                    .Include(c => c.Parent)
                    .Include(c => c.Children)
                    .Include(c => c.User)
                    .Include(c => c.Vendor)
                    .Include(c => c.Category)
                    .Include(c => c.Location)
                    .Include(c => c.LogEntries)
                    .ThenInclude(c => c.User)
                    .OrderBy(c => c.Id)
                    .Skip(page == 0 ? 0 : (page - 1) * 10)
                    .Take(10)
                    .Select(c => _itemUtilities.ItemToResponseDto(c))
                    .ToListAsync();
        }

        public async Task<IEnumerable<ItemResponseDto>> GetAllItemsByUserIdAsync(string id, int page)
        {
            return await _context.Items.Where(c => c.UserId == id)
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .Include(c => c.User)
                .Include(c => c.Vendor)
                .Include(c => c.Category)
                .Include(c => c.Location)
                .Include(c => c.LogEntries)!
                .ThenInclude(c => c.User)
                .OrderByDescending(c => c.CreatedDate)
                .Skip(page == 0 ? 0 : (page - 1) * 10)
                .Take(10)
                .Select(c => _itemUtilities.ItemToResponseDto(c))
                .ToListAsync();
        }

        public async Task<IEnumerable<ItemResponseDto>> GetChildrenAsync(string parentId)
        {
            return await _context.Items.Where(c => c.ParentId == parentId)
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .Include(c => c.User)
                .Include(c => c.Vendor)
                .Include(c => c.Category)
                .Include(c => c.Location)
                .Include(c => c.LogEntries)!
                .ThenInclude(c => c.User)
                .Select(c => _itemUtilities.ItemToResponseDto(c))
                .ToListAsync();
        }

        public async Task<ItemResponseDto> GetItemByIdAsync(string id)
        {
            var item = await _context.Items
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .Include(c => c.User)
                .Include(c => c.Vendor)
                .Include(c => c.Category)
                .Include(c => c.Location)
                .Include(c => c.LogEntries)!
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (item == null) return null;

            return _itemUtilities.ItemToResponseDto(item);
        }

        public async Task<List<string>?> CreateItemAsync(List<ItemCreateDto> itemsDto)
        {
            try
            {
                List<string> createdItemIds = new List<string>();
                foreach (var itemDto in itemsDto)
                {
                    Item item = new Item
                    {
                        WpId = itemDto.WpId,
                        UserId = itemDto.AddedById,
                        CategoryId = itemDto.CategoryId,
                        ParentId = itemDto.ParentId,
                        SerialNumber = itemDto.SerialNumber,
                        ProductNumber = itemDto.ProductNumber,
                        Type = itemDto.Type,
                        LocationId = itemDto.LocationId,
                        Description = itemDto.Description,
                        VendorId = itemDto.VendorId,
                        Comment = itemDto.Comment,
                        CreatedDate = DateTime.Now
                    };
                    await _context.Items.AddAsync(item);
                    createdItemIds.Add(item.Id);

                    LogEntry logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        UserId = itemDto.AddedById,
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

        public async Task UpdateItemAsync(string updatedById, ItemUpdateDto updatedItem)
        {
            var item = await _context.Items.Include(c => c.Category).FirstOrDefaultAsync(c => c.Id == updatedItem.Id);

            if (item != null)
            {
                LogEntry logEntry;
                if (updatedItem.WpId != item.WpId)
                {
                    logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        UserId = updatedById,
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
                        UserId = updatedById,
                        Message = "Serial number changed from " + item.SerialNumber + " to " + updatedItem.SerialNumber,
                        CreatedDate = DateTime.Now
                    };
                    item.SerialNumber = updatedItem.SerialNumber;
                    await _context.LogEntries.AddAsync(logEntry);
                }

                if (updatedItem.ProductNumber != item.ProductNumber)
                {
                    logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        UserId = updatedById,
                        Message = "Product number changed from " + item.ProductNumber + " to " + updatedItem.ProductNumber,
                        CreatedDate = DateTime.Now
                    };
                    item.ProductNumber = updatedItem.ProductNumber;
                    await _context.LogEntries.AddAsync(logEntry);
                }

                if (updatedItem.Type != item.Type)
                {
                    logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        UserId = updatedById,
                        Message = "Type changed from " + item.Type + " to " + updatedItem.Type,
                        CreatedDate = DateTime.Now
                    };
                    item.Type = updatedItem.Type;
                    await _context.LogEntries.AddAsync(logEntry);
                }

                if (updatedItem.CategoryId != item.CategoryId && updatedItem.CategoryId != null)
                {
                    var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == updatedItem.CategoryId);
                    logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        UserId = updatedById,
                        Message = "Category changed from " + item.Category?.Name + " to " + category.Name,
                        CreatedDate = DateTime.Now
                    };
                    item.CategoryId = updatedItem.CategoryId;
                    await _context.LogEntries.AddAsync(logEntry);
                }

                if (updatedItem.LocationId != item.LocationId && updatedItem.LocationId != null)
                {
                    var location = await _context.Locations.FirstOrDefaultAsync(c => c.Id == updatedItem.LocationId);
                    logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        UserId = updatedById,
                        Message = "Location changed from " + item.Location?.Name + " to " + location.Name,
                        CreatedDate = DateTime.Now
                    };
                    item.LocationId = updatedItem.LocationId;
                    await _context.LogEntries.AddAsync(logEntry);
                }

                if (updatedItem.Description != item.Description)
                {
                    logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        UserId = updatedById,
                        Message = "Description updated",
                        CreatedDate = DateTime.Now
                    };
                    item.Description = updatedItem.Description;
                    await _context.LogEntries.AddAsync(logEntry);
                }

                if (updatedItem.ParentId != item.ParentId)
                {
                    logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        UserId = updatedById,
                        Message = "Parent ID changed from " + item.ParentId + " to " + updatedItem.ParentId,
                        CreatedDate = DateTime.Now
                    };
                    item.ParentId = updatedItem.ParentId;
                    await _context.LogEntries.AddAsync(logEntry);
                }

                if (updatedItem.VendorId != item.VendorId && updatedItem.VendorId != null)
                {
                    var vendor = await _context.Vendors.FirstOrDefaultAsync(c => c.Id == updatedItem.VendorId);
                    logEntry = new LogEntry
                    {
                        ItemId = item.Id,
                        UserId = updatedById,
                        Message = "Vendor changed from " + item.Vendor?.Name + " to " + vendor.Name,
                        CreatedDate = DateTime.Now
                    };
                    item.VendorId = updatedItem.VendorId;
                    await _context.LogEntries.AddAsync(logEntry);
                }

                item.UserId = updatedItem.AddedById;
                item.Comment = updatedItem.Comment;
                item.ListId = updatedItem.ListId;
                item.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveParentIdAsync(string itemId)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemId);

            if (item != null)
            {
                item.ParentId = null;
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
