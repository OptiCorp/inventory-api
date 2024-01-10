using Inventory.Services;
using Inventory.Utilities;

namespace Inventory.Common
{
    public class ValidationHelper
    {
        private readonly IUserUtilities _userUtilities;
        
        public ValidationHelper(IUserUtilities userUtilities)
        {
            _userUtilities = userUtilities;
        }

        public bool BeValidStatus(string status)
        {
            return _userUtilities.IsValidStatus(status);
        }

    }
}