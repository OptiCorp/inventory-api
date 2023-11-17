using Inventory.Models;
using Inventory.Models.DTO;

namespace Inventory.Utilities
{
    public class SubassemblyUtilities : ISubassemblyUtilities
    {
        public SubassemblyResponseDto SubassemblyToResponseDto(Subassembly subassembly)
        {
            return new SubassemblyResponseDto
            {
                Id = subassembly.Id,
                WPId = subassembly.WPId,
                SerialNumber = subassembly.SerialNumber,
                ProductNumber = subassembly.ProductNumber,
                Location = subassembly.Location,
                Description = subassembly.Description,
                ParentAssemblyId = subassembly.AssemblyId,
                ParentSubassemblyId = subassembly.ParentSubassemblyId,
                Vendor = subassembly.Vendor,
                AddedById = subassembly.UserId,
                Comment = subassembly.Comment,
                CreatedDate = subassembly.CreatedDate,
                UpdatedDate = subassembly.UpdatedDate
            };
        }
    }
}
