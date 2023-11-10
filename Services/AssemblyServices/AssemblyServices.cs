using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Inventory.Models.DTO;
using Inventory.Utilities;

namespace Inventory.Services
{
    public class AssemblyService : IAssemblyService
    {
        public readonly InventoryDbContext _context;
        private readonly IAssemblyUtilities _assemblyUtilities;

        public AssemblyService(InventoryDbContext context, IAssemblyUtilities AssemblyUtilities)
        {
            _context = context;
            _assemblyUtilities = AssemblyUtilities;
        }

        public async Task<IEnumerable<AssemblyResponseDto>> GetAllAssembliesAsync()
        {
            return await _context.Assemblies.Select(c => _assemblyUtilities.AssemblyToResponseDto(c))
                                            .ToListAsync();
        }

        public async Task<AssemblyResponseDto> GetAssemblyByIdAsync(string id)
        {
            var assembly = await _context.Assemblies.FirstOrDefaultAsync(c => c.Id == id);

            if (assembly == null) return null;

            return _assemblyUtilities.AssemblyToResponseDto(assembly);
        }

        public async Task<string> CreateAssemblyAsync(AssemblyCreateDto assemblyDto)
        {
            var assembly = new Assembly
            {
                WPId = assemblyDto.WPId,
                SerialNumber = assemblyDto.SerialNumber,
                ProductNumber = assemblyDto.ProductNumber,
                Location = assemblyDto.Location,
                Description = assemblyDto.Description,
                UnitId = assemblyDto.UnitId,
                CreatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
            };

            await _context.Assemblies.AddAsync(assembly);
            await _context.SaveChangesAsync();

            return assembly.Id;
        }

        public async Task UpdateAssemblyAsync(AssemblyUpdateDto updatedAssembly)
        {
            var assembly = await _context.Assemblies.FirstOrDefaultAsync(c => c.Id == updatedAssembly.Id);

            if (assembly != null)
            {
                if (updatedAssembly.WPId != null)
                    assembly.WPId = updatedAssembly.WPId;

                if (updatedAssembly.SerialNumber != null)
                {
                    assembly.SerialNumber = updatedAssembly.SerialNumber;
                }
                if (updatedAssembly.ProductNumber != null)
                {
                    assembly.ProductNumber = updatedAssembly.ProductNumber;
                }
                if (updatedAssembly.DocumentationId != null)
                {
                    assembly.DocumentationId = updatedAssembly.DocumentationId;
                }
                if (updatedAssembly.Location != null)
                {
                    assembly.Location = updatedAssembly.Location;
                }
                if (updatedAssembly.Description != null)
                {
                    assembly.Description = updatedAssembly.Description;
                }
                if (updatedAssembly.UnitId != null)
                {
                    assembly.UnitId = updatedAssembly.UnitId;
                }

                assembly.UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAssemblyAsync(string id)
        {
            var assembly = await _context.Assemblies.FirstOrDefaultAsync(c => c.Id == id);
            if (assembly != null)
            {
                _context.Assemblies.Remove(assembly);
                await _context.SaveChangesAsync();
            }
        }
    }
}
