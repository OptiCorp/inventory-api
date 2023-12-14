using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.DTOs.CategoryDTOs
{
    public class CategoryUpdateDto
    {
        [Required]
        public string? Id { get; set; }
        
        [Required]
        public string? Name { get; set; }
    }
}