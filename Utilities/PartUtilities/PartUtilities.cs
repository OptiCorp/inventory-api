using Inventory.Models;
using Inventory.Models.DTO;

namespace Inventory.Utilities
{
    public class PartUtilities : IPartUtilities
    {
        public PartResponseDto PartToResponseDto(Part part)
        {
            return new PartResponseDto
            {
                Id = part.Id,
                WPId = part.WPId,
                SerialNumber = part.SerialNumber,
                ProductNumber = part.ProductNumber,
                Location = part.Location,
                Description = part.Description,
                ParentSubassemblyId = part.SubassemblyId,
                Vendor = part.Vendor,
                AddedById = part.UserId,
                Comment = part.Comment,
                CreatedDate = part.CreatedDate,
                UpdatedDate = part.UpdatedDate
            };
        }
    }
}
