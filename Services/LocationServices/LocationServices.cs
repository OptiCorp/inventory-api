using Inventory.Models;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                return await _context.Locations.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<IEnumerable<Location>> GetAllLocationsBySearchStringAsync(string searchString)
        {
            try
            {
                return await _context.Locations.Where(location => location.Name.Contains(searchString))
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<Location> GetLocationByIdAsync(string id)
        {
            try
            {
                return await _context.Locations.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<string?> CreateLocationAsync(Location locationCreate)
        {
            try
            {
                locationCreate.CreatedDate = DateTime.Now;
                await _context.Locations.AddAsync(locationCreate);
                await _context.SaveChangesAsync();
                return locationCreate.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task UpdateLocationAsync(Location locationUpdate)
        {
            try
            {
                var location = await _context.Locations.FirstOrDefaultAsync(c => c.Id == locationUpdate.Id);
        
                if (location != null)
                {
                    location.Name = locationUpdate.Name;
                    location.UpdatedDate = DateTime.Now;
        
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteLocationAsync(string id)
        {
            try
            {
                var location = await _context.Locations.FirstOrDefaultAsync(c => c.Id == id);
                if (location != null)
                {
                    _context.Locations.Remove(location);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
