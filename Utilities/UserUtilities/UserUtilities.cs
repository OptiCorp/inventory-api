using Inventory.Models;


namespace Inventory.Utilities
{
    public class UserUtilities : IUserUtilities
    {

        public bool IsValidStatus(string value)
        {
            string lowerCaseValue = value.ToLower();
            return lowerCaseValue == "active" || lowerCaseValue == "disabled" || lowerCaseValue == "deleted";
        }

        public static string GetUserStatus(UserStatus status)
        {
            switch (status)
            {
                case UserStatus.Active:
                    return "Active";
                case UserStatus.Disabled:
                    return "Disabled";
                case UserStatus.Deleted:
                    return "Deleted";
                default:
                    return "Active";
            }
        }

    }
}
