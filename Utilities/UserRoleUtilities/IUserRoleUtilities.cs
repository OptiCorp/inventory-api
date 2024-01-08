using Inventory.Models;


namespace Inventory.Utilities
{
    public interface IUserRoleUtilities
    {
        bool IsUserRoleNameTaken(IEnumerable<UserRole> userRoles, string userRoleName);
        bool IsValidUserRole(IEnumerable<UserRole> userRoles, string userRoleId);
    }
}