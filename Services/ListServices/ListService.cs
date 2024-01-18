using Inventory.Models;
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
                    .Include(c => c.Items)
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
                return await _context.Lists.Include(c => c.Items)
                    .ThenInclude(c => c.ItemTemplate)
                    .Where(c => c.CreatedById == userId)
                    .Where(list =>
                        list.Title.Contains(searchString) ||
                        list.Items.Any(item =>
                            item.WpId.Contains(searchString) ||
                            item.SerialNumber.Contains(searchString) ||
                            item.ItemTemplate.Description.Contains(searchString)
                        ))
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
                    .Include(c => c.Items)
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

        public async Task<List> GetListByIdAsync(string id)
        {
            try
            {
                return await _context.Lists
                    .Include(c => c.CreatedBy)
                    .Include(c => c.Items)
                    .ThenInclude(c => c.ItemTemplate)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<string?> CreateListAsync(List listCreate)
        {
            try
            {
                listCreate.CreatedDate = DateTime.Now;
                await _context.Lists.AddAsync(listCreate);
                await _context.SaveChangesAsync();
                return listCreate.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task AddItemsToListAsync(IEnumerable<string> itemIds, string listId)
        {
            try
            {
                foreach (var itemId in itemIds)
                {
                    var item = await _context.Items.FirstOrDefaultAsync(c => c.Id == itemId);
                    item.ListId = listId;
                }
            
                var list = await _context.Lists.FirstOrDefaultAsync(c => c.Id == listId);
                list.UpdatedDate = DateTime.Now;
            
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task RemoveItemsFromListAsync(IEnumerable<string> itemIds, string listId)
        {
            try
            {
                foreach (var itemId in itemIds)
                {
                    var item = await _context.Items.FirstOrDefaultAsync(c => c.Id == itemId);
                    item.ListId = null;
                }
            
                var list = await _context.Lists.FirstOrDefaultAsync(c => c.Id == listId);
                list.UpdatedDate = DateTime.Now;
            
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