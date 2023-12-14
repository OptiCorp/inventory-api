using Inventory.Models;
using Inventory.Models.DTOs.LocationDTOs;

namespace Inventory.Utilities
{
    public interface ILocationUtilities
    {
        public LocationResponseDto LocationToResponseDto(Location location);
    }
}