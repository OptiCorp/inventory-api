using Inventory.Models;
using Inventory.Models.DTO;

namespace Inventory.Utilities
{
    public class ItemUtilities : IItemUtilities
    {
        public ItemResponseDto ItemToResponseDto(Item item)
        {
            return new ItemResponseDto
            {
                Id = item.Id,
                WPId = item.WPId,
                SerialNumber = item.SerialNumber,
                ProductNumber = item.ProductNumber,
                Location = item.Location,
                Description = item.Description,
                SubassemblyId = item.SubassemblyId,
                Vendor = item.Vendor,
                AddedById = item.UserId,
                Comment = item.Comment,
                CreatedDate = item.CreatedDate,
                UpdatedDate = item.UpdatedDate
            };
        }
    }
}
