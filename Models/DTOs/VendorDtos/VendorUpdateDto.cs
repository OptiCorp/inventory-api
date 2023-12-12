using System.ComponentModel.DataAnnotations;
using Inventory.Models.DTO;

namespace Inventory.Models.DTOs.VendorDtos
{
    public class VendorUpdateDto
    {
        [Required]
        public string? Id { get; set; }
        
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
    }
}