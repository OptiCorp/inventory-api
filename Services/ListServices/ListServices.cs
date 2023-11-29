using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Inventory.Models.DTOs.ListDtos;
using Inventory.Utilities;

namespace Inventory.Services
{
    public class ListService : IListService
    {
        private readonly InventoryDbContext _context;
        private readonly IListUtilities _listUtilities;

        public ListService(InventoryDbContext context, IListUtilities listUtilities)
        {
            _context = context;
            _listUtilities = listUtilities;
        }

        public async Task<IEnumerable<ListResponseDto>> GetAllListsAsync()
        {
            return await _context.Lists.Select(c => _listUtilities.ListToResponseDto(c))
                                            .ToListAsync();
        }
        
        public async Task<IEnumerable<ListResponseDto>> GetAllListsBySearchStringAsync(string searchString, int page, string userId)
        {   
            if (page == 0)
            {
                return await _context.Lists.Include(c => c.Items)
                    .Where(c => c.UserId == userId)
                    .Where(list =>
                        list.Title.Contains(searchString) |
                        list.Items.Any(item =>
                        item.WpId.Contains(searchString) |
                        item.SerialNumber.Contains(searchString) |
                        item.Description.Contains(searchString)
                    ))
                    .OrderByDescending(c => c.CreatedDate)
                    .Take(10)
                    .Select(list => _listUtilities.ListToResponseDto(list))
                    .ToListAsync();
            }

            return await _context.Lists.Include(c => c.Items)
                .Where(c => c.UserId == userId)
                .Where(list =>
                    list.Title.Contains(searchString) |
                    list.Items.Any(item =>
                        item.WpId.Contains(searchString) |
                        item.SerialNumber.Contains(searchString) |
                        item.Description.Contains(searchString)
                    ))
                .OrderByDescending(c => c.CreatedDate)
                .Skip((page - 1) * 10)
                .Take(10)
                .Select(list => _listUtilities.ListToResponseDto(list))
                .ToListAsync();
        }
        
        public async Task<IEnumerable<ListResponseDto>> GetAllListsByUserIdAsync(string id, int page)
        {
            if (page == 0)
            {
                return await _context.Lists.Where(c => c.UserId == id)
                    .Include(c => c.Items)
                    .OrderByDescending(c => c.CreatedDate)
                    .Take(10)
                    .Select(c => _listUtilities.ListToResponseDto(c))
                    .ToListAsync();
            }
            return await _context.Lists.Where(c => c.UserId == id)
                .Include(c => c.Items)
                .OrderByDescending(c => c.CreatedDate)
                .Skip((page -1) * 10)
                .Take(10)
                .Select(c => _listUtilities.ListToResponseDto(c))
                .ToListAsync();
        }

        public async Task<ListResponseDto> GetListByIdAsync(string id)
        {
            var list = await _context.Lists
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == id);
        
            if (list == null) return null;
        
            return _listUtilities.ListToResponseDto(list);
        }
        
        public async Task<string?> CreateListAsync(ListCreateDto listCreateDto)
        {
            try
            {
                var list = new List
                {
                    Title = listCreateDto.Title,
                    UserId = listCreateDto.CreatedById,
                    CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now,
                        TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
                };
                
                await _context.Lists.AddAsync(list);
                await _context.SaveChangesAsync();
                return list.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine("Creating list failed.");
                return null;
            }
        }

        public async Task AddItemsToListAsync(IEnumerable<string> itemIds, string listId)
        {
            foreach (var itemId in itemIds)
            {
                var item = await _context.Items.FirstOrDefaultAsync(c => c.Id == itemId);
                item.ListId = listId;
            }
            await _context.SaveChangesAsync();
        }
        
        public async Task RemoveItemsFromListAsync(IEnumerable<string> itemIds)
        {
            foreach (var itemId in itemIds)
            {
                var item = await _context.Items.FirstOrDefaultAsync(c => c.Id == itemId);
                item.ListId = null;
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateListAsync(ListUpdateDto updatedList)
        {
            var list = await _context.Lists.FirstOrDefaultAsync(c => c.Id == updatedList.Id);
        
            if (list != null)
            {
                list.Title = updatedList.Title;
                list.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
        
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteListAsync(string id)
        {
            var list = await _context.Lists.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == id);
            if (list != null)
            {
                foreach (var item in list.Items)
                {
                    item.ListId = null;
                }
                    
                _context.Lists.Remove(list);
                await _context.SaveChangesAsync();
            }
        }
    }
}
