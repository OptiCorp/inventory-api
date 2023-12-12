using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.DTOs.CategoryDtos
{
    public class CategoryUpdateDto
    {
        [Required]
        public string? Id { get; set; }
        
        [Required]
        public string? Name { get; set; }
    }
}