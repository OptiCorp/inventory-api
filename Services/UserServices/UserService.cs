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
            return await _context.User
                            .Where(s => s.Status == UserStatus.Active)
                            .ToListAsync();
        }
        public async Task<IEnumerable<User>> GetAllUsersAdminAsync()
        {
            return await _context.User
                            .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string id)
        { 
            return await _context.User
                            .FirstOrDefaultAsync(u => u.Id == id);

        }

        public async Task<User> GetUserByAzureAdUserIdAsync(string azureAdUserId)
        {
            return await _context.User
                            .FirstOrDefaultAsync(u => u.AzureAdUserId == azureAdUserId);
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.User
                            .FirstOrDefaultAsync(u => u.Username == username);
        }
        
        public async Task<bool> IsUsernameTaken(string userName)
        {
            var users = await _context.User.ToListAsync();
            return users.Any(user => user.Username == userName);
        }

        public async Task<bool> IsEmailTaken(string userEmail)
        {
            var users = await _context.User.ToListAsync();
            return users.Any(user => user.Email == userEmail);
        }
    }
}
