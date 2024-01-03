using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.DTOs.VendorDTOs
{
    public class VendorCreateDto
    {
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? AddedById { get; set; }
    }
}