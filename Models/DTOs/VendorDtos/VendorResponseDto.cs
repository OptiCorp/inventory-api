using System.ComponentModel.DataAnnotations;
using Inventory.Models.DTO;

namespace Inventory.Models.DTOs.VendorDtos
{
    public class VendorResponseDto
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
        
        [Required]
        public string? AddedById { get; set; }
        
        [Required]
        public string? CreatedDate { get; set; }

        public string? UpdatedDate { get; set; }
        
        public UserDto? User { get; set; }
    }
}