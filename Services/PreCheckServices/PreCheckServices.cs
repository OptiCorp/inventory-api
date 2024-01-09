using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services
{
    public class PreCheckServices : IPreCheckService
    {
        private readonly InventoryDbContext _context;

        public PreCheckServices(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PreCheck>> GetAllPreChecksAsync()
        {
            return await _context.PreChecks.ToListAsync();
        }
        
        public async Task<PreCheck> GetPreCheckByIdAsync(string id)
        {
            return await _context.PreChecks.FirstOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task<string?> CreatePreCheckAsync(PreCheck preCheckCreate)
        {
            try
            {
                await _context.PreChecks.AddAsync(preCheckCreate);
                await _context.SaveChangesAsync();
                return preCheckCreate.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task UpdatePreCheckAsync(PreCheck preCheckUpdate)
        {
            var preCheck = await _context.PreChecks.FirstOrDefaultAsync(c => c.Id == preCheckUpdate.Id);
        
            if (preCheck != null)
            {
                preCheck.Check = preCheckUpdate.Check;
                preCheck.Comment = preCheckUpdate.Comment;
        
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePreCheckAsync(string id)
        {
            var preCheck = await _context.PreChecks.FirstOrDefaultAsync(c => c.Id == id);
            if (preCheck != null)
            {
                _context.PreChecks.Remove(preCheck);
                await _context.SaveChangesAsync();
            }
        }
    }
}
