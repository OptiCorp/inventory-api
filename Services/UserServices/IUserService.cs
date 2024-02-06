using Inventory.Models;

namespace Inventory.Services;

public interface IUserService
{
    Task<bool> IsUsernameTaken(string userName);
    Task<bool> IsEmailTaken(string userEmail);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByUsernameAsync(string name);
    Task<User?> GetUserByAzureAdUserIdAsync(string azureAdUserId);
    Task<User?> GetUserByIdAsync(string id);
}