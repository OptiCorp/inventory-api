using Inventory.Models;


namespace Inventory.Services
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRole>> GetAllUserRolesAsync();
        Task<UserRole> GetUserRoleByIdAsync(string id);
        Task<string> CreateUserRoleAsync(UserRole userRole);
        Task UpdateUserRoleAsync(UserRole userRole);
        Task DeleteUserRoleAsync(string id);
        Task<bool> IsUserRoleInUseAsync(UserRole userRole);
    }
}