using Inventory.Models;

namespace Inventory.Services
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetAllLocationsAsync();
        Task<IEnumerable<Location>> GetAllLocationsBySearchStringAsync(string searchString);
        Task<Location> GetLocationByIdAsync(string id);
        Task<string?> CreateLocationAsync(Location location);
        Task UpdateLocationAsync(Location location);
        Task DeleteLocationAsync(string id);
    }
}