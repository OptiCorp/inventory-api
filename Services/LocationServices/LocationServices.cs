using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Inventory.Utilities;

namespace Inventory.Services
{
    public class LocationService : ILocationService
    {
        private readonly InventoryDbContext _context;

        public LocationService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await _context.Locations.ToListAsync();
        }
        
        public async Task<IEnumerable<Location>> GetAllLocationsBySearchStringAsync(string searchString)
        {   
            return await _context.Locations.Where(location => location.Name.Contains(searchString))
                                            .ToListAsync();
        }
        
        public async Task<Location> GetLocationByIdAsync(string id)
        {
            return await _context.Locations.FirstOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task<string?> CreateLocationAsync(Location locationCreate)
        {
            try
            {
                await _context.Locations.AddAsync(locationCreate);
                await _context.SaveChangesAsync();
                return locationCreate.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task UpdateLocationAsync(Location locationUpdate)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(c => c.Id == locationUpdate.Id);
        
            if (location != null)
            {
                location.Name = locationUpdate.Name;
                location.UpdatedDate = DateTime.Now;
        
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteLocationAsync(string id)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(c => c.Id == id);
            if (location != null)
            {
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
            }
        }
    }
}
