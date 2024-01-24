using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services
{
    public class ListService : IListService
    {
        private readonly InventoryDbContext _context;

        public ListService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<List>> GetAllListsAsync()
        {
            try
            {
                return await _context.Lists.Include(c => c.CreatedBy)
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
                return await _context.Lists.Include(c => c.Items)!
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
                return await _context.Lists.Where(c => c.CreatedById == id)
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
                return await _context.Lists
                    .Include(c => c.CreatedBy)
                    .Include(c => c.Items)!
                    .ThenInclude(c => c.ItemTemplate)
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

                await _context.Lists.AddAsync(list);
                await _context.SaveChangesAsync();
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
                foreach (var itemId in itemIds)
                {
                    var item = await _context.Items.Include(item => item.Children).FirstOrDefaultAsync(c => c.Id == itemId);
                    if (item != null) item.ListId = listId;

                    var subItemIds = new List<string>();

                    if (addSubItems == true)
                    {
                        if (item?.Children != null)
                            foreach (var child in item.Children)
                            {
                                if (child.Id != null) subItemIds.Add(child.Id);
                            }

                        await AddItemsToListAsync(subItemIds, listId, addSubItems);
                    }
                }

                var list = await _context.Lists.FirstOrDefaultAsync(c => c.Id == listId);
                if (list != null) list.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
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
                foreach (var itemId in itemIds)
                {
                    var item = await _context.Items.Include(item => item.Children).FirstOrDefaultAsync(c => c.Id == itemId);
                    if (item != null) item.ListId = null;

                    var subItemIds = new List<string>();

                    if (removeSubItems == true)
                    {
                        if (item?.Children != null)
                            foreach (var child in item.Children)
                            {
                                if (child.Id != null) subItemIds.Add(child.Id);
                            }

                        await RemoveItemsFromListAsync(subItemIds, listId, removeSubItems);
                    }
                }

                var list = await _context.Lists.FirstOrDefaultAsync(c => c.Id == listId);
                if (list != null) list.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
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
                var list = await _context.Lists.FirstOrDefaultAsync(c => c.Id == listUpdate.Id);

                if (list != null)
                {
                    list.Title = listUpdate.Title;
                    list.UpdatedDate = DateTime.Now;

                    await _context.SaveChangesAsync();
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
                var list = await _context.Lists.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == id);
                if (list != null)
                {
                    if (list.Items != null)
                        foreach (var item in list.Items)
                        {
                            item.ListId = null;
                        }

                    _context.Lists.Remove(list);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
