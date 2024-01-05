using Inventory.Models;

namespace Inventory.Utilities
{
    public interface IUserUtilities
    {
        bool IsUsernameTaken(IEnumerable<User> users, string username);

        bool IsEmailTaken(IEnumerable<User> users, string userEmail);

        bool IsValidStatus(string value);

    }
}
