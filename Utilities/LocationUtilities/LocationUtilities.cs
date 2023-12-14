using Inventory.Models;
using Inventory.Models.DTOs.LocationDTOs;

namespace Inventory.Utilities
{
    public class LocationUtilities : ILocationUtilities
    {
        private readonly IUserUtilities _userUtilities;
        
        public LocationUtilities(IUserUtilities userUtilities)
        {
            _userUtilities = userUtilities;
        }
        public LocationResponseDto LocationToResponseDto(Location location)
        {
            return new LocationResponseDto
            {
                Id = location.Id,
                Name = location.Name,
                AddedById = location.UserId,
                CreatedDate = location.CreatedDate.HasValue ? location.CreatedDate+"Z": null,
                UpdatedDate = location.UpdatedDate.HasValue ? location.UpdatedDate+"Z": null,
                User = location.User != null ? _userUtilities.UserToDto(location.User) : null
            };
        }
    }
}