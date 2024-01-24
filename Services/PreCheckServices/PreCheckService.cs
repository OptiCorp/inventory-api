using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services
{
    public class PreCheckService : IPreCheckService
    {
        private readonly InventoryDbContext _context;

        public PreCheckService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PreCheck>> GetAllPreChecksAsync()
        {
            try
            {
                return await _context.PreChecks.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<PreCheck?> GetPreCheckByIdAsync(string id)
        {
            try
            {
                return await _context.PreChecks.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<string?> CreatePreCheckAsync(PreCheckCreateDto preCheckCreate)
        {
            try
            {
                var preCheck = new PreCheck()
                {
                    Check = preCheckCreate.Check,
                    Comment = preCheckCreate.Comment
                };

                await _context.PreChecks.AddAsync(preCheck);
                await _context.SaveChangesAsync();
                return preCheck.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task UpdatePreCheckAsync(PreCheck preCheckUpdate)
        {
            try
            {
                var preCheck = await _context.PreChecks.FirstOrDefaultAsync(c => c.Id == preCheckUpdate.Id);

                if (preCheck != null)
                {
                    preCheck.Check = preCheckUpdate.Check;
                    preCheck.Comment = preCheckUpdate.Comment;

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeletePreCheckAsync(string id)
        {
            try
            {
                var preCheck = await _context.PreChecks.FirstOrDefaultAsync(c => c.Id == id);
                if (preCheck != null)
                {
                    _context.PreChecks.Remove(preCheck);
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
