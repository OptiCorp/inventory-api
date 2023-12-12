using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.DTOs.CategoryDtos
{
    public class CategoryCreateDto
    {
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? AddedById { get; set; }
    }
}