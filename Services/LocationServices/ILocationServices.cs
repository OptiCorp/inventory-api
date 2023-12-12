using Inventory.Models.DTOs.LocationDtos;

namespace Inventory.Services
{
    public interface ILocationService
    {
        Task<IEnumerable<LocationResponseDto>> GetAllLocationsAsync();
        Task<IEnumerable<LocationResponseDto>> GetAllLocationsBySearchStringAsync(string searchString);
        Task<LocationResponseDto> GetLocationByIdAsync(string id);
        Task<string?> CreateLocationAsync(LocationCreateDto item);
        Task UpdateLocationAsync(LocationUpdateDto item);
        Task DeleteLocationAsync(string id);
    }
}