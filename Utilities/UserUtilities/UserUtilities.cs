using Inventory.Models;


namespace Inventory.Utilities;

public class UserUtilities : IUserUtilities
{

    public bool IsValidStatus(string value)
    {
        var lowerCaseValue = value.ToLower();
        return lowerCaseValue is "active" or "disabled" or "deleted";
    }

    public static string GetUserStatus(UserStatus status)
    {
        return status switch
        {
            UserStatus.Active => "Active",
            UserStatus.Disabled => "Disabled",
            UserStatus.Deleted => "Deleted",
            _ => "Active"
        };
    }

}