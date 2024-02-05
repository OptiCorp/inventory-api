using Inventory.Utilities;

namespace Inventory.Common;

public class ValidationHelper(IUserUtilities userUtilities)
{
    public bool BeValidStatus(string status)
    {
        return userUtilities.IsValidStatus(status);
    }

}