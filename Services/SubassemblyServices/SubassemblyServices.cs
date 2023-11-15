using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Inventory.Models.DTO;
using Inventory.Utilities;

namespace Inventory.Services
{
    public class SubassemblyService : ISubassemblyService
    {
        public readonly InventoryDbContext _context;
        private readonly ISubassemblyUtilities _subassemblyUtilities;

        public SubassemblyService(InventoryDbContext context, ISubassemblyUtilities subassemblyUtilities)
        {
            _context = context;
            _subassemblyUtilities = subassemblyUtilities;
        }

        public async Task<IEnumerable<SubassemblyResponseDto>> GetAllSubassembliesAsync()
        {
            return await _context.Subassemblies.Select(c => _subassemblyUtilities.SubassemblyToResponseDto(c))
                                            .ToListAsync();
        }

        public async Task<SubassemblyResponseDto> GetSubassemblyByIdAsync(string id)
        {
            var subassembly = await _context.Subassemblies.FirstOrDefaultAsync(c => c.Id == id);

            if (subassembly == null) return null;

            return _subassemblyUtilities.SubassemblyToResponseDto(subassembly);
        }

        public async Task<string> CreateSubassemblyAsync(SubassemblyCreateDto subassemblyDto)
        {
            var subassembly = new Subassembly
            {
                WPId = subassemblyDto.WPId,
                SerialNumber = subassemblyDto.SerialNumber,
                ProductNumber = subassemblyDto.ProductNumber,
                Location = subassemblyDto.Location,
                Description = subassemblyDto.Description,
                AssemblyId = subassemblyDto.ParentAssemblyId,
                SubassemblyId = subassemblyDto.ParentSubassemblyId,
                Vendor = subassemblyDto.Vendor,
                UserId = subassemblyDto.AddedById,
                Comment = subassemblyDto.Comment,
                CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
            };

            await _context.Subassemblies.AddAsync(subassembly);
            await _context.SaveChangesAsync();

            return subassembly.Id;
        }

        public async Task UpdateSubassemblyAsync(SubassemblyUpdateDto updatedSubassembly)
        {
            var subassembly = await _context.Subassemblies.FirstOrDefaultAsync(c => c.Id == updatedSubassembly.Id);

            if (subassembly != null)
            {
                if (updatedSubassembly.WPId != null)
                    subassembly.WPId = updatedSubassembly.WPId;

                if (updatedSubassembly.SerialNumber != null)
                {
                    subassembly.SerialNumber = updatedSubassembly.SerialNumber;
                }
                if (updatedSubassembly.ProductNumber != null)
                {
                    subassembly.ProductNumber = updatedSubassembly.ProductNumber;
                }
                if (updatedSubassembly.DocumentationId != null)
                {
                    subassembly.DocumentationId = updatedSubassembly.DocumentationId;
                }
                if (updatedSubassembly.Location != null)
                {
                    subassembly.Location = updatedSubassembly.Location;
                }
                if (updatedSubassembly.Description != null)
                {
                    subassembly.Description = updatedSubassembly.Description;
                }
                if (updatedSubassembly.ParentAssemblyId != null)
                {
                    subassembly.AssemblyId = updatedSubassembly.ParentAssemblyId;
                }
                if (updatedSubassembly.ParentSubassemblyId != null)
                {
                    subassembly.SubassemblyId = updatedSubassembly.ParentSubassemblyId;
                }
                if (updatedSubassembly.Vendor != null)
                {
                    subassembly.Vendor = updatedSubassembly.Vendor;
                }
                if (updatedSubassembly.AddedById != null)
                {
                    subassembly.UserId = updatedSubassembly.AddedById;
                }
                if (updatedSubassembly.Comment != null)
                {
                    subassembly.Comment = updatedSubassembly.Comment;
                }

                subassembly.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteSubassemblyAsync(string id)
        {
            var subassembly = await _context.Subassemblies.FirstOrDefaultAsync(c => c.Id == id);
            if (subassembly != null)
            {
                _context.Subassemblies.Remove(subassembly);
                await _context.SaveChangesAsync();
            }
        }
    }
}
