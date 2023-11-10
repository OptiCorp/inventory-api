using Inventory.Models;
using Inventory.Models.DTO;

namespace Inventory.Utilities
{
    public class UnitUtilities : IUnitUtilities
    {
        public UnitResponseDto UnitToResponseDto(Unit unit)
        {
            return new UnitResponseDto
            {
                Id = unit.Id,
                WPId = unit.WPId,
                SerialNumber = unit.SerialNumber,
                ProductNumber = unit.ProductNumber,
                Location = unit.Location,
                Description = unit.Description,
                CreatedDate = unit.CreatedDate,
                UpdatedDate = unit.UpdatedDate
            };
        }
    }
}
