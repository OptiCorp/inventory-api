using System.ComponentModel.DataAnnotations;
using Inventory.Models;

namespace Inventory.Services
{
    public enum DeleteMode
    {
        [Display(Name = "Soft")]
        Soft,
        [Display(Name = "Hard")]
        Hard
    }

    public interface IUserService
    {
        Task<bool> IsUsernameTaken(string userName);
        Task<bool> IsEmailTaken(string userEmail);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByUsernameAsync(string name);
        Task<User> GetUserByAzureAdUserIdAsync(string azureAdUserId);
        Task<User> GetUserByIdAsync(string id);
        Task UpdateUserAsync(User user);
        Task<string> CreateUserAsync(User user);
        Task DeleteUserAsync(string id);
        Task HardDeleteUserAsync(string id);
    }
}