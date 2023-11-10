using Inventory.Models;
using Inventory.Models.DTO;


namespace Inventory.Utilities
{
    public interface IUnitUtilities
    {
        public UnitResponseDto UnitToResponseDto(Unit unit);
    }
}
