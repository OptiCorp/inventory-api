using Inventory.Models;
using Inventory.Models.DTOs.ListDtos;


namespace Inventory.Utilities
{
    public interface IListUtilities
    {
        public ListResponseDto ListToResponseDto(List list);
    }
}