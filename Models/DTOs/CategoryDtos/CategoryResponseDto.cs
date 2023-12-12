using System.ComponentModel.DataAnnotations;
using Inventory.Models.DTO;

namespace Inventory.Models.DTOs.CategoryDtos
{
    public class CategoryResponseDto
    {
        [Required]
        public string? Id { get; set; }
        
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? AddedById { get; set; }
        
        [Required]
        public string? CreatedDate { get; set; }

        public string? UpdatedDate { get; set; }
        
        public UserDto? User { get; set; }
    }
}