using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.DTOs.DocumentationDtos
{
    public class DocumentationCreateDto
    {
        [Required]
        public string? ItemId { get; set; }
        
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public IFormFile[]? Files { get; set; }
    }
}

