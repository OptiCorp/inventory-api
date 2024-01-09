using Microsoft.EntityFrameworkCore;
using Inventory.Models;
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
        
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.User
                            .Include(u => u.UserRole)
                            .Where(s => s.Status == UserStatus.Active)
                            .ToListAsync();
        }
        public async Task<IEnumerable<User>> GetAllUsersAdminAsync()
        {
            return await _context.User
                            .Include(u => u.UserRole)
                            .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string id)
        { 
            return await _context.User
                            .Include(u => u.UserRole)
                            .FirstOrDefaultAsync(u => u.Id == id);

        }

        public async Task<User> GetUserByAzureAdUserIdAsync(string azureAdUserId)
        {
            return await _context.User
                            .Include(u => u.UserRole)
                            .FirstOrDefaultAsync(u => u.AzureAdUserId == azureAdUserId);
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.User
                            .Include(u => u.UserRole)
                            .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<string> CreateUserAsync(User userCreate)
        {   
            userCreate.CreatedDate = DateTime.Now;
            _context.User.Add(userCreate);
            await _context.SaveChangesAsync();

            return userCreate.Id;
        }
        public async Task UpdateUserAsync(User userUpdate)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userUpdate.Id);
            if (user != null)
            {
                if (userUpdate.Username != null)
                    user.Username = userUpdate.Username;
                if (userUpdate.FirstName != null)
                    user.FirstName = userUpdate.FirstName;
                if (userUpdate.LastName != null)
                    user.LastName = userUpdate.LastName;
                if (userUpdate.Email != null)
                    user.Email = userUpdate.Email;
                if (userUpdate.UserRoleId != null)
                    user.UserRoleId = userUpdate.UserRoleId;
                if (userUpdate.Status != null)
                {
                    string status = userUpdate.Status.ToString().ToLower();
                    switch (status)
                    {
                        case "active":
                            user.Status = UserStatus.Active;
                            break;
                        case "disabled":
                            user.Status = UserStatus.Disabled;
                            break;
                        case "deleted":
                            user.Status = UserStatus.Deleted;
                            break;
                    }
                }
                user.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteUserAsync(string id)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                user.Status = UserStatus.Deleted;
                await _context.SaveChangesAsync();
            }
        }
        public async Task HardDeleteUserAsync(string id)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        
    }
}
