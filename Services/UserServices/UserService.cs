using Microsoft.EntityFrameworkCore;
using Inventory.Models;

namespace Inventory.Services
{
    public class UserService : IUserService
    {
        private readonly InventoryDbContext _context;

        public UserService(InventoryDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.User
                            .Where(s => s.Status == UserStatus.Active)
                            .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<IEnumerable<User>> GetAllUsersAdminAsync()
        {
            try
            {
                return await _context.User.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            try
            {
                return await _context.User.FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<User?> GetUserByAzureAdUserIdAsync(string azureAdUserId)
        {
            try
            {
                return await _context.User.FirstOrDefaultAsync(u => u.AzureAdUserId == azureAdUserId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            try
            {
                return await _context.User.FirstOrDefaultAsync(u => u.Username == username);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<bool> IsUsernameTaken(string userName)
        {
            try
            {
                var users = await _context.User.ToListAsync();
                return users.Any(user => user.Username == userName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> IsEmailTaken(string userEmail)
        {
            try
            {
                var users = await _context.User.ToListAsync();
                return users.Any(user => user.Email == userEmail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
