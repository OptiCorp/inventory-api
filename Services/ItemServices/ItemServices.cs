using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Inventory.Models.DTO;
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
            return await _context.Items.Select(c => _itemUtilities.ItemToResponseDto(c))
                                            .ToListAsync();
        }

        public async Task<IEnumerable<ItemResponseDto>> GetAllItemsBySearchStringAsync(string searchString)
        {
            return await _context.Items.Where(c => c.WpId.Contains(searchString) | c.SerialNumber.Contains(searchString) | c.Description.Contains(searchString))
                                            .Select(c => _itemUtilities.ItemToResponseDto(c))
                                            .ToListAsync();
        }
        
        public async Task<IEnumerable<ItemResponseDto>> GetAllItemsByUserIdAsync(string id)
        {
            return await _context.Items.Where(c => c.UserId == id)
                .OrderByDescending(c => c.CreatedDate)
                .Select(c => _itemUtilities.ItemToResponseDto(c))
                .ToListAsync();
        }

        public async Task<IEnumerable<ItemResponseDto>> GetChildrenAsync(string parentId)
        {
            return await _context.Items.Where(c => c.ParentId == parentId)
                .Select(c => _itemUtilities.ItemToResponseDto(c))
                .ToListAsync();
        }

        public async Task<ItemResponseDto> GetItemByIdAsync(string id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(c => c.Id == id);
        
            if (item == null) return null;
        
            return _itemUtilities.ItemToResponseDto(item);
        }
        
        public async Task<string> CreateItemAsync(ItemCreateDto itemDto)
        {
            var item = new Item
            {
                WpId = itemDto.WpId,
                SerialNumber = itemDto.SerialNumber,
                ProductNumber = itemDto.ProductNumber,
                Type = itemDto.Type,
                Location = itemDto.Location,
                Description = itemDto.Description,
                ParentId = itemDto.ParentId,
                Vendor = itemDto.Vendor,
                UserId = itemDto.AddedById,
                Comment = itemDto.Comment,
                CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
            };
        
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
        
            return item.Id;
        }

        public async Task UpdateItemAsync(ItemUpdateDto updatedItem)
        {
            var item = await _context.Items.FirstOrDefaultAsync(c => c.Id == updatedItem.Id);
        
            if (item != null)
            {
                item.WpId = updatedItem.WpId;
                item.SerialNumber = updatedItem.SerialNumber;
                item.ProductNumber = updatedItem.ProductNumber;
                item.Type = updatedItem.Type;
                item.Location = updatedItem.Location;
                item.Description = updatedItem.Description;
                item.ParentId = updatedItem.ParentId;
                item.Vendor = updatedItem.Vendor;
                item.UserId = updatedItem.AddedById;
                item.Comment = updatedItem.Comment;
        
                item.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
        
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteItemAsync(string id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(c => c.Id == id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
