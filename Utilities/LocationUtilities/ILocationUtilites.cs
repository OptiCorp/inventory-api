using Inventory.Models;
using Inventory.Models.DTOs.LocationDtos;

namespace Inventory.Utilities
{
    public interface ILocationUtilities
    {
        public LocationResponseDto LocationToResponseDto(Location location);
    }
}