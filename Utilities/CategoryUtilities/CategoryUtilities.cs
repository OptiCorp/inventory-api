using Inventory.Models;
using Inventory.Models.DTOs.CategoryDtos;

namespace Inventory.Utilities
{
    public class CategoryUtilities : ICategoryUtilities
    {
        private readonly IUserUtilities _userUtilities;
        
        public CategoryUtilities(IUserUtilities userUtilities)
        {
            _userUtilities = userUtilities;
        }
        public CategoryResponseDto CategoryToResponseDto(Category category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                AddedById = category.UserId,
                CreatedDate = category.CreatedDate.HasValue ? category.CreatedDate+"Z": null,
                UpdatedDate = category.UpdatedDate.HasValue ? category.UpdatedDate+"Z": null,
                User = category.User != null ? _userUtilities.UserToDto(category.User) : null,
            };
        }
    }
}