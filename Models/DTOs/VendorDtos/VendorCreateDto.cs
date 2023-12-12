using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.DTOs.VendorDtos
{
    public class VendorCreateDto
    {
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? Address { get; set; }
        
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        
        [Required]
        [Phone]
        public string? PhoneNumber { get; set; }
        
        [Required]
        public string? AddedById { get; set; }
    }
}