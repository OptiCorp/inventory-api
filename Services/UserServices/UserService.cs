using Inventory.Models;
using Inventory.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Inventory.Utilities;

namespace Inventory.Services
{
    public class UserService : IUserService
    {
        private readonly InventoryDbContext _context;
        private readonly IUserUtilities _userUtilities;

        public UserService(InventoryDbContext context, IUserUtilities userUtilities)
        {
            _context = context;
            _userUtilities = userUtilities;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _context.User
                            .Where(s => s.Status == UserStatus.Active)
                            .Select(u => _userUtilities.UserToDto(u))
                            .ToListAsync();
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersAdminAsync()
        {
            return await _context.User
                            .Select(u => _userUtilities.UserToDto(u))
                            .ToListAsync();
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            var user = await _context.User
                            .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;

            return _userUtilities.UserToDto(user);
        }

        public async Task<User> GetUserByAzureAdUserIdAsync(string id)
        {
            return await _context.User
                            .FirstOrDefaultAsync(u => u.AzureAdUserId == id);
        }
        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            var user = await _context.User
                            .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return null;

            return _userUtilities.UserToDto(user);
        }
    }
}
