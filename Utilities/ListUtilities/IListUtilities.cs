using Inventory.Models;
using Inventory.Models.DTOs.ListDTOs;

namespace Inventory.Utilities
{
    public interface IListUtilities
    {
        public ListResponseDto ListToResponseDto(List list);
    }
}