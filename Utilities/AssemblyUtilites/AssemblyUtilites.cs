using Inventory.Models;
using Inventory.Models.DTO;

namespace Inventory.Utilities
{
    public class AssemblyUtilities : IAssemblyUtilities
    {
        public AssemblyResponseDto AssemblyToResponseDto(Assembly assembly)
        {
            return new AssemblyResponseDto
            {
                Id = assembly.Id,
                WPId = assembly.WPId,
                SerialNumber = assembly.SerialNumber,
                ProductNumber = assembly.ProductNumber,
                Location = assembly.Location,
                Description = assembly.Description,
                ParentUnitId = assembly.UnitId,
                Vendor = assembly.Vendor,
                AddedById = assembly.UserId,
                Comment = assembly.Comment,
                CreatedDate = assembly.CreatedDate,
                UpdatedDate = assembly.UpdatedDate
            };
        }
    }
}
