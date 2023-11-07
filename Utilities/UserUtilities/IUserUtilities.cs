using Inventory.Models;
using Inventory.Models.DTO;


namespace Inventory.Utilities
{
    public interface IUserUtilities
    {
        bool IsUsernameTaken(IEnumerable<UserDto> users, string username);

        bool IsEmailTaken(IEnumerable<UserDto> users, string userEmail);

        bool IsValidStatus(string value);

        UserDto UserToDto(User user);
    }
}
