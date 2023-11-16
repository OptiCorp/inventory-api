using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Inventory.Models.DTO;
using Inventory.Utilities;

namespace Inventory.Services
{
    public class ItemService : IItemService
    {
        public readonly InventoryDbContext _context;
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

        public async Task<IEnumerable<ItemResponseDto>> GetAllItemsBySubassemblyIdAsync(string subassemblyId)
        {
            return await _context.Items.Where(c => c.SubassemblyId == subassemblyId)
                                            .Select(c => _itemUtilities.ItemToResponseDto(c))
                                            .ToListAsync();
        }

        public async Task<IEnumerable<ItemResponseDto>> GetAllItemsBySearchStringAsync(string searchString)
        {
            return await _context.Items.Where(c => c.WPId.Contains(searchString) | c.SerialNumber.Contains(searchString) | c.Description.Contains(searchString))
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
                WPId = itemDto.WPId,
                SerialNumber = itemDto.SerialNumber,
                ProductNumber = itemDto.ProductNumber,
                Location = itemDto.Location,
                Description = itemDto.Description,
                SubassemblyId = itemDto.ParentSubassemblyId,
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
                if (updatedItem.WPId != null)
                    item.WPId = updatedItem.WPId;

                if (updatedItem.SerialNumber != null)
                {
                    item.SerialNumber = updatedItem.SerialNumber;
                }
                if (updatedItem.ProductNumber != null)
                {
                    item.ProductNumber = updatedItem.ProductNumber;
                }
                if (updatedItem.DocumentationId != null)
                {
                    item.DocumentationId = updatedItem.DocumentationId;
                }
                if (updatedItem.Location != null)
                {
                    item.Location = updatedItem.Location;
                }
                if (updatedItem.Description != null)
                {
                    item.Description = updatedItem.Description;
                }
                if (updatedItem.ParentSubassemblyId != null)
                {
                    item.SubassemblyId = updatedItem.ParentSubassemblyId;
                }
                if (updatedItem.Vendor != null)
                {
                    item.Vendor = updatedItem.Vendor;
                }
                if (updatedItem.AddedById != null)
                {
                    item.UserId = updatedItem.AddedById;
                }
                if (updatedItem.Comment != null)
                {
                    item.Comment = updatedItem.Comment;
                }

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
