using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Inventory.Models.DTO;
using Inventory.Utilities;

namespace Inventory.Services
{
    public class UnitService : IUnitService
    {
        public readonly InventoryDbContext _context;
        private readonly IUnitUtilities _unitUtilities;

        public UnitService(InventoryDbContext context, IUnitUtilities unitUtilities)
        {
            _context = context;
            _unitUtilities = unitUtilities;
        }

        public async Task<IEnumerable<UnitResponseDto>> GetAllUnitsAsync()
        {
            return await _context.Units.Select(c => _unitUtilities.UnitToResponseDto(c))
                                            .ToListAsync();
        }

        public async Task<UnitResponseDto> GetUnitByIdAsync(string id)
        {
            var unit = await _context.Units.FirstOrDefaultAsync(c => c.Id == id);

            if (unit == null) return null;

            return _unitUtilities.UnitToResponseDto(unit);
        }

        public async Task<string> CreateUnitAsync(UnitCreateDto unitDto)
        {
            var unit = new Unit
            {
                WPId = unitDto.WPId,
                SerialNumber = unitDto.SerialNumber,
                ProductNumber = unitDto.ProductNumber,
                Location = unitDto.Location,
                Description = unitDto.Description,
                Vendor = unitDto.Vendor,
                UserId = unitDto.AddedById,
                Comment = unitDto.Comment,
                CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
            };

            await _context.Units.AddAsync(unit);
            await _context.SaveChangesAsync();

            return unit.Id;
        }

        public async Task UpdateUnitAsync(UnitUpdateDto updatedUnit)
        {
            var unit = await _context.Units.FirstOrDefaultAsync(c => c.Id == updatedUnit.Id);

            if (unit != null)
            {
                if (updatedUnit.WPId != null)
                    unit.WPId = updatedUnit.WPId;

                if (updatedUnit.SerialNumber != null)
                {
                    unit.SerialNumber = updatedUnit.SerialNumber;
                }
                if (updatedUnit.ProductNumber != null)
                {
                    unit.ProductNumber = updatedUnit.ProductNumber;
                }
                if (updatedUnit.DocumentationId != null)
                {
                    unit.DocumentationId = updatedUnit.DocumentationId;
                }
                if (updatedUnit.Location != null)
                {
                    unit.Location = updatedUnit.Location;
                }
                if (updatedUnit.Description != null)
                {
                    unit.Description = updatedUnit.Description;
                }
                if (updatedUnit.Vendor != null)
                {
                    unit.Vendor = updatedUnit.Vendor;
                }
                if (updatedUnit.AddedById != null)
                {
                    unit.UserId = updatedUnit.AddedById;
                }
                if (updatedUnit.Comment != null)
                {
                    unit.Comment = updatedUnit.Comment;
                }

                unit.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUnitAsync(string id)
        {
            var unit = await _context.Units.FirstOrDefaultAsync(c => c.Id == id);
            if (unit != null)
            {
                _context.Units.Remove(unit);
                await _context.SaveChangesAsync();
            }
        }
    }
}
