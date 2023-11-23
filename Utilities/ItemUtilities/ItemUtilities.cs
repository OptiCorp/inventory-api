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
                WpId = item.WpId,
                SerialNumber = item.SerialNumber,
                ProductNumber = item.ProductNumber,
                Type = item.Type,
                Location = item.Location,
                Description = item.Description,
                ParentId = item.ParentId,
                Vendor = item.Vendor,
                AddedById = item.UserId,
                Comment = item.Comment,
                CreatedDate = item.CreatedDate,
                UpdatedDate = item.UpdatedDate
            };
        }
    }
}
