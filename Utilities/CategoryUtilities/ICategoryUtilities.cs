using Inventory.Models;
using Inventory.Models.DTOs.CategoryDtos;

namespace Inventory.Utilities
{
    public interface ICategoryUtilities
    {
        public CategoryResponseDto CategoryToResponseDto(Category category);
    }
}