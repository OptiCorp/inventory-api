using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.DTOs.CategoryDTOs
{
    public class CategoryCreateDto
    {
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? AddedById { get; set; }
    }
}