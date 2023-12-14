using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Inventory.Models.DTOs.LocationDTOs;
using Inventory.Utilities;

namespace Inventory.Services
{
    public class LocationService : ILocationService
    {
        private readonly InventoryDbContext _context;
        private readonly ILocationUtilities _locationUtilities;

        public LocationService(InventoryDbContext context, ILocationUtilities locationUtilities)
        {
            _context = context;
            _locationUtilities = locationUtilities;
        }

        public async Task<IEnumerable<LocationResponseDto>> GetAllLocationsAsync()
        {
            return await _context.Locations.Include(c => c.User)
                                            .Select(c => _locationUtilities.LocationToResponseDto(c))
                                            .ToListAsync();
        }
        
        public async Task<IEnumerable<LocationResponseDto>> GetAllLocationsBySearchStringAsync(string searchString)
        {   
            return await _context.Locations.Where(location => location.Name.Contains(searchString))
                                            .Include(c => c.User)
                                            .Select(location => _locationUtilities.LocationToResponseDto(location))
                                            .ToListAsync();
        }
        
        public async Task<LocationResponseDto> GetLocationByIdAsync(string id)
        {
            var location = await _context.Locations.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
        
            if (location == null) return null;
        
            return _locationUtilities.LocationToResponseDto(location);
        }
        
        public async Task<string?> CreateLocationAsync(LocationCreateDto locationCreateDto)
        {
            try
            {
                var location = new Location
                {
                    Name = locationCreateDto.Name,
                    UserId = locationCreateDto.AddedById,
                    CreatedDate = DateTime.Now
                };
                
                await _context.Locations.AddAsync(location);
                await _context.SaveChangesAsync();
                return location.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task UpdateLocationAsync(LocationUpdateDto updatedLocation)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(c => c.Id == updatedLocation.Id);
        
            if (location != null)
            {
                location.Name = updatedLocation.Name;
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
