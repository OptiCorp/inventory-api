using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Inventory.Models.DTO;
using Inventory.Utilities;

namespace Inventory.Services
{
    public class PartService : IPartService
    {
        public readonly InventoryDbContext _context;
        private readonly IPartUtilities _partUtilities;

        public PartService(InventoryDbContext context, IPartUtilities partUtilities)
        {
            _context = context;
            _partUtilities = partUtilities;
        }

        public async Task<IEnumerable<PartResponseDto>> GetAllPartsAsync()
        {
            return await _context.Parts.Select(c => _partUtilities.PartToResponseDto(c))
                                            .ToListAsync();
        }

        public async Task<IEnumerable<PartResponseDto>> GetAllPartsBySubassemblyIdAsync(string subassemblyId)
        {
            return await _context.Parts.Where(c => c.SubassemblyId == subassemblyId)
                                            .Select(c => _partUtilities.PartToResponseDto(c))
                                            .ToListAsync();
        }

        public async Task<IEnumerable<PartResponseDto>> GetAllPartsBySearchStringAsync(string searchString)
        {
            return await _context.Parts.Where(c => c.WPId.Contains(searchString) | c.SerialNumber.Contains(searchString) | c.Description.Contains(searchString))
                                            .Select(c => _partUtilities.PartToResponseDto(c))
                                            .ToListAsync();
        }

        public async Task<PartResponseDto> GetPartByIdAsync(string id)
        {
            var part = await _context.Parts.FirstOrDefaultAsync(c => c.Id == id);

            if (part == null) return null;

            return _partUtilities.PartToResponseDto(part);
        }

        public async Task<string> CreatePartAsync(PartCreateDto partDto)
        {
            var part = new Part
            {
                WPId = partDto.WPId,
                SerialNumber = partDto.SerialNumber,
                ProductNumber = partDto.ProductNumber,
                Location = partDto.Location,
                Description = partDto.Description,
                SubassemblyId = partDto.ParentSubassemblyId,
                Vendor = partDto.Vendor,
                UserId = partDto.AddedById,
                Comment = partDto.Comment,
                CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
            };

            await _context.Parts.AddAsync(part);
            await _context.SaveChangesAsync();

            return part.Id;
        }

        public async Task UpdatePartAsync(PartUpdateDto updatedPart)
        {
            var part = await _context.Parts.FirstOrDefaultAsync(c => c.Id == updatedPart.Id);

            if (part != null)
            {
                if (updatedPart.WPId != null)
                    part.WPId = updatedPart.WPId;

                if (updatedPart.SerialNumber != null)
                {
                    part.SerialNumber = updatedPart.SerialNumber;
                }
                if (updatedPart.ProductNumber != null)
                {
                    part.ProductNumber = updatedPart.ProductNumber;
                }
                if (updatedPart.DocumentationId != null)
                {
                    part.DocumentationId = updatedPart.DocumentationId;
                }
                if (updatedPart.Location != null)
                {
                    part.Location = updatedPart.Location;
                }
                if (updatedPart.Description != null)
                {
                    part.Description = updatedPart.Description;
                }
                if (updatedPart.ParentSubassemblyId != null)
                {
                    part.SubassemblyId = updatedPart.ParentSubassemblyId;
                }
                if (updatedPart.Vendor != null)
                {
                    part.Vendor = updatedPart.Vendor;
                }
                if (updatedPart.AddedById != null)
                {
                    part.UserId = updatedPart.AddedById;
                }
                if (updatedPart.Comment != null)
                {
                    part.Comment = updatedPart.Comment;
                }

                part.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePartAsync(string id)
        {
            var part = await _context.Parts.FirstOrDefaultAsync(c => c.Id == id);
            if (part != null)
            {
                _context.Parts.Remove(part);
                await _context.SaveChangesAsync();
            }
        }
    }
}
