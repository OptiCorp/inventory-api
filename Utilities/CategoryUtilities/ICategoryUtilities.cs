using Inventory.Models;
using Inventory.Models.DTOs.CategoryDTOs;

namespace Inventory.Utilities
{
    public interface ICategoryUtilities
    {
        public CategoryResponseDto CategoryToResponseDto(Category category);
    }
}