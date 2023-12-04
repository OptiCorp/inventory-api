using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Inventory.Models.DTOs.ItemDtos;
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

        public async Task<IEnumerable<ItemResponseDto>> GetAllItemsBySearchStringAsync(string searchString, int page)
        {   
            if (page == 0)
            {
                return await _context.Items.Where(c => c.WpId.Contains(searchString) || c.SerialNumber.Contains(searchString) || c.Description.Contains(searchString))
                    .Include(c => c.Parent)
                    .Include(c => c.Children)
                    .Include(c => c.User)
                    .OrderBy(c => c.Id)
                    .Take(10)
                    .Select(c => _itemUtilities.ItemToResponseDto(c))
                    .ToListAsync();
            }

            return await _context.Items.Where(c => c.WpId.Contains(searchString) || c.SerialNumber.Contains(searchString) || c.Description.Contains(searchString))
                                            .Include(c => c.Parent)
                                            .Include(c => c.Children)
                                            .Include(c => c.User)
                                            .OrderBy(c => c.Id)
                                            .Skip((page -1) * 10)
                                            .Take(10)
                                            .Select(c => _itemUtilities.ItemToResponseDto(c))
                                            .ToListAsync();
        }
        
        public async Task<IEnumerable<ItemResponseDto>> GetAllItemsByUserIdAsync(string id, int page)
        {
            if (page == 0)
            {
                return await _context.Items.Where(c => c.UserId == id)
                    .Include(c => c.Parent)
                    .Include(c => c.Children)
                    .Include(c => c.User)
                    .OrderByDescending(c => c.CreatedDate)
                    .Take(10)
                    .Select(c => _itemUtilities.ItemToResponseDto(c))
                    .ToListAsync();
            }
            return await _context.Items.Where(c => c.UserId == id)
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedDate)
                .Skip((page -1) * 10)
                .Take(10)
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
            var item = await _context.Items
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .Include((c => c.User))
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
                        ParentId = itemDto.ParentId,
                        SerialNumber = itemDto.SerialNumber,
                        ProductNumber = itemDto.ProductNumber,
                        Type = itemDto.Type,
                        Location = itemDto.Location,
                        Description = itemDto.Description,
                        Vendor = itemDto.Vendor,
                        Comment = itemDto.Comment,
                        CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now,
                            TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
                    };
                    await _context.Items.AddAsync(item);
                    await _context.SaveChangesAsync();
                    createdItemIds.Add(item.Id);
                }
                return createdItemIds;
            }
            catch (Exception e)
            {
                Console.WriteLine("Creating item(s) failed.");
                return null;
            }
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
    }
}
